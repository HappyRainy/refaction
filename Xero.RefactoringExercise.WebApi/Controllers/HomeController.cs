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
            return Ok(new { message= "This is home page of Xero refactoring exercise project. System will populate some sample data when first time initialized. For all PUT, POST, DELETE routes, please set AuthTick in request header to value 1F7A570C-9764-41E5-9F0E-212FA2C703AC to be authenticated from the system." });
        }
    }
}
