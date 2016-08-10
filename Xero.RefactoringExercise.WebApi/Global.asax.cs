using System.Web.Http;
using Xero.RefactoringExercise.WebApi.App_Start;

namespace Xero.RefactoringExercise.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("Allow");
            Response.Headers.Remove("X-Sourcefiles");
        }
    }
}
