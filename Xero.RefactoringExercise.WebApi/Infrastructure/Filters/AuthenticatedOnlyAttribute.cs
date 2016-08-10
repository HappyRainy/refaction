using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ninject;
using Xero.RefactoringExercise.Domain.Services;
using Xero.RefactoringExercise.WebApi.Infrastructure.Exceptions;

namespace Xero.RefactoringExercise.WebApi.Infrastructure.Filters
{
    /// <summary>
    /// A very simple authenticated only filter to protect actions only accessible by authenticated users
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthenticatedOnlyAttribute : AuthorizationFilterAttribute
    {
        [Inject]
        public IKernel Kernel { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // Ensure properly scoped services are used
            var userContextService = Kernel.Get<IUserContextService>();

            var userContext = userContextService.Current;

            if (userContext == null)
                throw new ForbiddenException(
                    "The client identity must be authenticated to access the given route. Please set AuthTicket in request header to 1F7A570C-9764-41E5-9F0E-212FA2C703AC for this exersice.");
        }
    }
}