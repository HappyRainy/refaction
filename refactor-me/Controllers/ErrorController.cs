using System.Web.Http;
using NLog;
using Xero.RefactoringExercise.WebApi.Controllers.Support;

namespace Xero.RefactoringExercise.WebApi.Controllers
{
    public class ErrorController : ControllerBase
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions, AcceptVerbs("PATCH")]
        public IHttpActionResult Error404(string url)
        {
            _log.Fatal($"404 Not Found: {url}");

            return NotFound();
        }
    }
}