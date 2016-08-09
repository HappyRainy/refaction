using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Moq;
using Xero.RefactoringExercise.Domain.Services;
using Xero.RefactoringExercise.Tests.Support;
using Xero.RefactoringExercise.WebApi.Controllers.Support;

namespace Xero.RefactoringExercise.Tests.WebApi.Controllers
{
    public class ControllerTestBase : UnitTestBase
    {
        protected void SetupRequest(ControllerBase controller)
        {
            var request = new HttpRequestMessage();

            controller.Request = request;

            Kernel.Inject(controller);
        }
    }
}
