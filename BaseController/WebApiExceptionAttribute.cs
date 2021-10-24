using Common.Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicController
{
    internal class WebApiExceptionAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        protected ResultInfo resultInfo = new ResultInfo();

        public override void OnException(ExceptionContext context)
        {
            JsonResult result = null;

            if (context.Exception is ReloginException)
            {
                result = resultInfo.Unlogin(context.Exception.Message);
            }
            else
                result = resultInfo.Fail(context.Exception.Message);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in context.ModelState)
            {
                stringBuilder.Append($"{item.Key}={item.Value.RawValue.ToString()},");
            }

            string message = $"{context.ActionDescriptor.DisplayName}，the data : {stringBuilder.ToString().Trim(',')}.";

            LoggerHelper.GlobalLogger.Error(message, context.Exception);

            context.Result = result;
            base.OnException(context);
        }
    }
}
