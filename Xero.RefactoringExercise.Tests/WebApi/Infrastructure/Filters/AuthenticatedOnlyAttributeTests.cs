using System.Net.Http;
using System.Web.Http.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xero.RefactoringExercise.Domain.Context;
using Xero.RefactoringExercise.Domain.Services;
using Xero.RefactoringExercise.Tests.Support;
using Xero.RefactoringExercise.WebApi.Infrastructure.Exceptions;
using Xero.RefactoringExercise.WebApi.Infrastructure.Filters;

namespace Xero.RefactoringExercise.Tests.WebApi.Infrastructure.Filters
{
    [TestClass]
    public class AuthenticatedOnlyAttributeTests : UnitTestBase
    {
        IUserContext _userContext;
        Mock<IUserContextService> _userContextService;
        AuthenticatedOnlyAttribute _attribute;

        [TestInitialize]
        public void Init()
        {
            _userContext = new AuthenticatedUserContext("Jing");
            _userContextService = new Mock<IUserContextService>(MockBehavior.Strict);

            Kernel.RebindMock(_userContextService);

            _attribute = new AuthenticatedOnlyAttribute
            {
                Kernel = Kernel
            };

        }

        [TestMethod]
        [ExpectedException(typeof(ForbiddenException))]
        public void UnAuthenticatedUserCauses403Forbidden()
        {
            _userContextService.SetupGet(x => x.Current).Returns((IUserContext)null).Verifiable();

            var actionContext = MakeContext();

            _attribute.OnAuthorization(actionContext);

            _userContextService.Verify();
        }

        [TestMethod]
        public void AssertionPresentLetsRequestContinue()
        {
            _userContextService.SetupGet(x => x.Current).Returns(_userContext).Verifiable();
            var actionContext = MakeContext();

            _attribute.OnAuthorization(actionContext);

            Assert.IsNull(actionContext.Response);
            _userContextService.Verify();

        }

        static HttpActionContext MakeContext()
        {
            return new HttpActionContext
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestMessage()
                }
            };
        }
    }
}
