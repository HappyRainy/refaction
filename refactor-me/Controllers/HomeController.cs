using System.Web.Http;
using Xero.RefactoringExercise.WebApi.Controllers.Support;

namespace Xero.RefactoringExercise.WebApi.Controllers
{
    public class HomeController : ControllerBase
    {
        [Route("~/")] //Specifies that this is the default action for the entire application. Route: /
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok(new { message="This is home page of Xero refactoring exercise project"});
        }
    }
}
