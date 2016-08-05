using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Parameters;
using Ninject.Web.WebApi;
using Xero.RefactoringExercise.WebApi.Infrastructure;

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
                 ContractResolver = new CamelCasePropertyNamesContractResolver(),
                 TypeNameHandling = TypeNameHandling.None, // Potentially security problem if not "None"?
                 ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                 DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                 NullValueHandling = NullValueHandling.Ignore,
                 Formatting = Formatting.None
             };

             // Add exception handling
             config.Services.Replace(typeof(IExceptionHandler), kernel.Get<XeroRefactoringExerciseExceptionHandler>());

             // Enable CORS support
             config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

             var configArgument = new ConstructorArgument("configuration", config);
             var activatorArgument = new ConstructorArgument("instance", config.Services.GetService(typeof(IHttpControllerActivator)));

            var controllerActivator = kernel.Get<LoggingControllerActivator>(activatorArgument);
            var controllerSelector = kernel.Get<HttpNotFoundAwareHttpControllerSelector>(configArgument);
            //var controllerActionSelector = kernel.Get<HttpNotFoundAwareControllerActionSelector>();

            config.Services.Replace(typeof(IHttpControllerActivator), controllerActivator);
            config.Services.Replace(typeof(IHttpControllerSelector), controllerSelector);
            /*config.Services.Replace(typeof(IHttpActionSelector), controllerActionSelector);*/

            // Use Ninject for all dependencies
            config.DependencyResolver = new NinjectDependencyResolver(NinjectWebCommon.Kernel);

            WebApiRouteConfig.RegisterRoutes(config);
        }
    }
}
