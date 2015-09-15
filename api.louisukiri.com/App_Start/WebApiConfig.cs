using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using api.louisukiri.com.Extensions;
using Autofac;
using Autofac.Integration.WebApi;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity.bot;
using cicd.infrastructure;
using godaddy.domains.cicd.Controllers;
using godaddy.domains.cicd.Models;

namespace godaddy.domains.cicd
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SetupIocResolver(config);
            config.ParameterBindingRules.Add(z =>
            {
                if (z.ParameterType == typeof(RequestPayload))
                {
                    return new PayloadParameterBinding(z);
                }
                return null;
            });
            config.ParameterBindingRules.Add(z =>
            {
                if (z.ParameterType == typeof (TestdataPayload))
                {
                    return new TestdataParameterBinding(z);
                }
                return null;
            });
            config.ParameterBindingRules.Add(z =>
            {
                if (z.ParameterType == typeof(GenericApiEvent))
                {
                    return new GenericEventParameterBinding(z);
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

        private static void SetupIocResolver(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SlackBot>().As<IBot>();
            //builder.RegisterType<SlackBotBrain>().As<IBrain>();
            builder.RegisterType<SlackBotPersonality>().As<IPersonality>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);


            var container = builder.Build();
            builder.Register(z=> new BotController(container.Resolve<IBot>()));

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            
        }
    }
}
