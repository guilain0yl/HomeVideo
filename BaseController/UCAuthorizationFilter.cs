using Common.Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using HomeVideo.Util;
using Util;

namespace BasicController
{
    public class UCAuthorizationFilter : IAuthorizationFilter
    {
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                return;

            // 若使用严格意义的角色拦截 可以使用布隆过滤器 进行拦截 菜单ID 自增数字

            var request = context.HttpContext.Request;
            var authorization = request.Headers["Authorization"].ToString();

            if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Result = (JsonResult)ResultInfo.Unlogin("无效令牌信息。");
            }

            if (TokenInfo.DescToken(authorization, out var key, out var expiresIn))
            {
                if (key.IsNullOrEmpty() || !CacheHelper.SessionCache.KeyExists(key))
                    throw new ReloginException("无效令牌信息。");

                if (DateTime.Now.GetUnixTimestamp() > expiresIn)
                    throw new ReloginException("用户信息已过期，请重新登陆。");

                ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                claimsIdentity.AddClaim(new Claim(ConstantHelper.ClaimKey, key));
                context.HttpContext.User.AddIdentity(claimsIdentity);

                return;
            }

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Result = (JsonResult)ResultInfo.Unlogin("用户信息已过期，请重新登陆。");
        }

        ResultInfo ResultInfo = new ResultInfo();
    }
}
