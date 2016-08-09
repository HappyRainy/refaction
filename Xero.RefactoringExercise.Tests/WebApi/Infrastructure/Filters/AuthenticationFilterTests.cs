using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
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
    public class AuthenticationFilterTests : UnitTestBase
    {
        Mock<IUserContextService> _userContextService;
        AuthenticationFilter _filter;

        [TestInitialize]
        public void Init()
        {
            _userContextService = new Mock<IUserContextService>(MockBehavior.Strict);

            Kernel.RebindMock(_userContextService);

            _filter = new AuthenticationFilter(Kernel);
        }

        [TestMethod]
        public async Task NoTicketLeadsNullUserContext()
        {
            var context = MakeAuthContext();
            await _filter.AuthenticateAsync(context, new CancellationToken());
            Assert.IsNull(context.ErrorResult);
            Assert.IsNull(context.Principal);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedException))]
        public void EmptyTicketIsUnauthorized()
        {
            var context = MakeAuthContext("");
            _filter.AuthenticateAsync(context, new CancellationToken());
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedException))]
        public void TicketNotFoundIsUnauthorized()
        {
            var context = MakeAuthContext("STERCES");

            _userContextService.Setup(x => x.GetByTicket("STERCES")).Returns((IUserContext)null);

            _filter.AuthenticateAsync(context, new CancellationToken());
        }

        [TestMethod]
        public async Task ValidTicketContinues()
        {

            var context = MakeAuthContext("STERCES");
            var authenticated = new SimpleUserContext("Jing");

            _userContextService.Setup(x => x.GetByTicket("STERCES")).Returns(authenticated);
            _userContextService.Setup(x => x.Login(authenticated));

            await _filter.AuthenticateAsync(context, new CancellationToken());

            Assert.IsNull(context.ErrorResult);
            Assert.IsNotNull(context.Principal);

            _userContextService.Verify(x => x.Login(authenticated), Times.Once);
        }

        static HttpAuthenticationContext MakeAuthContext(string authTicket = null)
        {
            var actionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestMessage()
                }
            };

            if (authTicket != null)
                actionContext.Request.Headers.Add(AuthenticationFilter.AuthTicketKey, authTicket);

            return new HttpAuthenticationContext(actionContext, null);
        }
    }
}
