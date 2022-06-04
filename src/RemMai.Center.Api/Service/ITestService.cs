﻿using SmartCat.DynamicWebApi;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RemMai.Center.Api.Service;

public interface ITestService
{
    string GetService();
}

[DynamicWebApi]
public class TestService : ITestService, IDynamicWebApi
{
    [HttpGet]
    public string GetService()
    {
        throw new Exception("测试错误");
        var data = SmartCat.Cat.HttpContext;
        Console.WriteLine("333");
        return "Data";
    }
}
