using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace api.louisukiri.com.Extensions
{
    public static class ApiInfrastructure
    {
        public static string GetRequestBody(this HttpActionContext context)
        {
            if (context.Request.Content == null)
            {
                return string.Empty;
            }
            return context.Request
                .Content
                .ReadAsStringAsync()
                .Result;
        }
        public static Task SetActionParameter<T>(this HttpActionContext context, string parameterName, T value)
        {
            context.ActionArguments[parameterName] = value;
            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);

            return tsc.Task;
        }
    }
}