using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Xero.RefactoringExercise.WebApi.Infrastructure.Exceptions
{
    /// <summary>
    /// Indicates that the current user did not have permission to perform the requested action.
    /// </summary>
    public class ForbiddenException : Exception, IResponseException
    {
        public ForbiddenException(string format, params object[] args)
            : base(string.Format(format, args))
        {
            _clientMessage = string.Format(format, args);
        }

        public HttpStatusCode Code => HttpStatusCode.Forbidden;

        private readonly string _clientMessage;

        public string ClientMessage => _clientMessage;
    }
}