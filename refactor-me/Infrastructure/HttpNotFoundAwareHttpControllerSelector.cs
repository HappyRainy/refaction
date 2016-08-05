using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Xero.RefactoringExercise.WebApi.Infrastructure
{
    /// <summary>
    /// Direct to Error controller if cannot find the specified controller
    /// </summary>
    public class HttpNotFoundAwareHttpControllerSelector : DefaultHttpControllerSelector
    {
        public HttpNotFoundAwareHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor descriptor;

            try
            {
                descriptor = base.SelectController(request);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;
                if (code != HttpStatusCode.NotFound) throw;

                var routeData = request.GetRouteData();
                var routeValues = routeData.Values;
                routeValues["controller"] = "Error";
                routeValues["action"] = "Error404";

                descriptor = base.SelectController(request);
            }

            return descriptor;
        }
    }
}