using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using api.louisukiri.com.Extensions;
using cicdDomain.cicd.infrastructure;

namespace api.louisukiri.com
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

          config.ParameterBindingRules.Add(z =>
          {
            if (z.ParameterType == typeof (RequestPayload))
            {
              return new PayloadParameterBinding(z);
            }
            return null;
          });
            // Web API configuration and services
            config.Formatters.Remove(config.Formatters.FormUrlEncodedFormatter);

            config.Services.Replace(typeof(IContentNegotiator),
                new DefaultContentNegotiator(excludeMatchOnTypeOnly: true));


            //// Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}"
            );
        }
    }
}
