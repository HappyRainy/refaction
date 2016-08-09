using System;
using System.Collections.Generic;
using NLog;
using Xero.RefactoringExercise.Domain.Context;

namespace Xero.RefactoringExercise.Domain.Services
{
    public class SimpleUserContextService : IUserContextService
    {
        //Dummy User Context for testing purpose only
        private static Dictionary<string, IUserContext> DummyTestingUserContexts = new Dictionary<string, IUserContext>
        {
            { "1F7A570C-9764-41E5-9F0E-212FA2C703AC", new SimpleUserContext ("XeroTestingUser")} 
        };

        private readonly AppContext _context;
        internal const string UserContextKey = "UserContext";
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public SimpleUserContextService(AppContext context)
        {
            _context = context;
        }

        public IUserContext Current
        {
            get
            {
                object item;

                if (_context.Items.TryGetValue(UserContextKey, out item))
                {
                    var userContext = item as IUserContext;

                    if (userContext != null)
                        return userContext;
                }

                return null;
            }
            private set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                _context.Items[UserContextKey] = value;

                _log.Debug($"Current user context set to {value}");
            }
        }

        public IUserContext GetByTicket(string ticket)
        {
            if (string.IsNullOrWhiteSpace(ticket)) throw new ArgumentNullException(nameof(ticket));

            IUserContext userContext = null;

            if(DummyTestingUserContexts.ContainsKey(ticket))
                userContext = DummyTestingUserContexts[ticket];
            
            if (userContext == null)
                _log.Warn($"User context with ticket {ticket} does not exist");

            return userContext;
        }

        /// <summary>
        /// Set user context to current
        /// </summary>
        /// <param name="userContext"></param>
        public void Login(IUserContext userContext)
        {
            if (userContext == null) throw new ArgumentNullException(nameof(userContext));

            Current = userContext;
        }

    }
}
