﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat;

namespace RemMai.Center.Api.Handler;

public class JwtHandler : AuthorizationHandler
{
    public override Task<bool> IsAuth(AuthorizationHandlerContext context)
    {
        return Task.FromResult(true);
    }
}
   