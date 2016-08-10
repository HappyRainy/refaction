using System.Web.Http;

namespace Xero.RefactoringExercise.WebApi.Controllers.Support
{
    /// <summary>
    /// Controller base for all controllers in WebApi 
    /// </summary>
    public abstract class ControllerBase : ApiController
    {
        protected ControllerBase()
        {
            
        }
    }
}