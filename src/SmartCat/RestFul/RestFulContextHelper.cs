using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Helpers;
using SmartCat.Model;
using System.Reflection;

namespace SmartCat.RestFul;

public static class RestFulContextHelper
{
    /// <summary>
    /// 参数检查并且序列化参数的错误信息。
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ActionExecutingContext DataValidation(this ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            List<string> errors = new List<string>();

            foreach (var item in context.ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            RestFulResult<object> result = new RestFulResult<object>()
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "请求参数错误，详细信息请查看Error",
                Error = errors
            };
            context.Result = new JsonResult(result);
        }

        return context;
    }
    /// <summary>
    ///  判断跳过RestFul序列化响应
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static bool SkipRestFul(this ActionExecutedContext context)
    {
        var controller = (context.ActionDescriptor as ControllerActionDescriptor);
        var method = controller.MethodInfo;
        var type = controller.ControllerTypeInfo;
        return SkipRestFul(method.SkipRestFulByMothodInfo(), type.SkipRestFulByTypeInfo());
    }

    /// <summary>
    /// 跳过RestFul序列化响应
    /// </summary>
    /// <param name="methodEnable"></param>
    /// <param name="typeEnable"></param>
    /// <returns></returns>
    public static bool SkipRestFul(bool? methodEnable, bool? typeEnable)
    {
        if (methodEnable.HasValue)
        {
            return methodEnable.Value;
        }
        else
        {
            return typeEnable.HasValue && typeEnable.Value;
        }
    }


    public static ExceptionContext RestFulException(this ExceptionContext context)
    {
        if (context.Exception != null)
        {
            var result = new RestFulResult<object>()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Success = false,
                Message = context.Exception.Message,
            };

            if (context.Exception is SmartCatException exception)
            {
                result.StatusCode = exception.StatusCode;
            }

            context.Result = new JsonResult(result);
            context.HttpContext.Response.StatusCode = result.StatusCode;
            context.ExceptionHandled = true;
        }
        return context;
    }


    public static bool? SkipRestFulByMothodInfo(this MethodInfo methodInfo)
    {
        NonRestFulAttribute attr = methodInfo.GetSingleAttributeOrNull<NonRestFulAttribute>();
        if (attr == null)
        {
            return null;
        }
        else
        {
            return attr.Enable;
        }
    }
    public static bool? SkipRestFulByTypeInfo(this TypeInfo type)
    {
        NonRestFulAttribute attr = type.GetSingleAttributeOrNull<NonRestFulAttribute>();
        if (attr == null)
        {
            return null;
        }
        else
        {
            return attr.Enable;
        }
    }
}
