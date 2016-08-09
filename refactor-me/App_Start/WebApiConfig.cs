using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Parameters;
using Ninject.Web.WebApi;
using Ninject.Web.WebApi.FilterBindingSyntax;
using Xero.RefactoringExercise.WebApi.Infrastructure;
using Xero.RefactoringExercise.WebApi.Infrastructure.Filters;

namespace Xero.RefactoringExercise.WebApi.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var kernel = NinjectWebCommon.Kernel;

            // Disable XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Remove(config.Formatters.FormUrlEncodedFormatter);
            config.Formatters.Remove(config.Formatters.OfType<JQueryMvcFormUrlEncodedFormatter>().FirstOrDefault());

            // Set JSON serialiser settings
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = TypeNameHandling.None, 
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                Culture = new CultureInfo(string.Empty)
                {
                    NumberFormat = new NumberFormatInfo
                    {
                        CurrencyDecimalDigits = 2
                    }
                }
            };

            // Add exception handling
            config.Services.Replace(typeof (IExceptionHandler), kernel.Get<XeroRefactoringExerciseExceptionHandler>());

            // Enable CORS support
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            //Global filter
            kernel.BindHttpFilter<AuthenticationFilter>(FilterScope.Global);


            var configArgument = new ConstructorArgument("configuration", config);
            var activatorArgument = new ConstructorArgument("instance", config.Services.GetService(typeof (IHttpControllerActivator)));

            var controllerActivator = kernel.Get<LoggingControllerActivator>(activatorArgument);
            var controllerSelector = kernel.Get<HttpNotFoundAwareHttpControllerSelector>(configArgument);

            config.Services.Replace(typeof (IHttpControllerActivator), controllerActivator);
            config.Services.Replace(typeof(IHttpControllerSelector), controllerSelector);

            // Use Ninject for all dependencies
            config.DependencyResolver = new NinjectDependencyResolver(NinjectWebCommon.Kernel);

            WebApiRouteConfig.RegisterRoutes(config);
        }
    }
}
