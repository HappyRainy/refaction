using System.Web.Http;

namespace Xero.RefactoringExercise.WebApi
{
    public class WebApiRouteConfig
    {
        public static void RegisterRoutes(HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();

            var routes = configuration.Routes;

            // Convention-based routing.
            MapCatchAllApi404(routes);
        }

        static void MapCatchAllApi404(HttpRouteCollection routes)
        {
            routes.MapHttpRoute("Error404",
                "{*url}",
                new { controller = "Error", action = "Error404" });
        }
    }
}