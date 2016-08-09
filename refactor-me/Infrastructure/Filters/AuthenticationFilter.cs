using System;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Ninject;
using NLog;
using Xero.RefactoringExercise.Domain.Context;
using Xero.RefactoringExercise.Domain.Helpers;
using Xero.RefactoringExercise.Domain.Services;
using Xero.RefactoringExercise.WebApi.Infrastructure.Exceptions;

namespace Xero.RefactoringExercise.WebApi.Infrastructure.Filters
{
    /// <summary>
    /// Authenticates the client using the optional AuthTicket header in the request.
    /// </summary>
    /// <remarks>
    /// 
    /// If there is no AuthTicket head supplied , the request will be set up using an anonymous 
    /// <see cref="IUserContext" />.  
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {
        public const string AuthTicketKey = "AuthTicket";

        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        readonly IKernel _kernel;

        public AuthenticationFilter(IKernel kernel)
        {
            _kernel = kernel;
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var userContextService = _kernel.Get<IUserContextService>();

            IUserContext userContext;

            // If the auth header was specified, lookup, validate, and set current user context
            if (context.Request.Headers.Contains(AuthTicketKey))
            {
                // Only get the first value from header
                var authTicket = context.Request.Headers
                    .GetValues(AuthTicketKey)
                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(authTicket))
                    throw new UnauthorizedException("An empty auth ticket was supplied.");

                // Look up the user context using the existing auth ticket
                userContext = userContextService.GetByTicket(authTicket);

                if (userContext == null)
                    throw new UnauthorizedException("Could not find any user context matching the given auth ticket.");

                userContextService.Login(userContext);

                SetPrincipal(context, userContext);
            }

            return TaskHelpers.Completed();
        }

        private static void SetPrincipal(HttpAuthenticationContext context, IUserContext userContext)
        {
            var identity = new GenericIdentity(userContext.IdentityName.ToString(CultureInfo.InvariantCulture));
            var principal = new GenericPrincipal(identity, new string[] {});

            context.Principal = principal;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            // This is where we could issue an authentication challenge back to the client on 401.
            return TaskHelpers.Completed();
        }
    }
}
