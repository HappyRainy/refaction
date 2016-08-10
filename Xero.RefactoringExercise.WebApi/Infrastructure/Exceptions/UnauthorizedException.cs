using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;

namespace Xero.RefactoringExercise.WebApi.Infrastructure.Exceptions
{
    public class UnauthorizedException : Exception, IResponseException
    {
        public UnauthorizedException(string format, params object[] args)
            : base(string.Format(format, args))
        {
            _clientMessage = string.Format(format, args);
        }

        public HttpStatusCode Code => HttpStatusCode.Unauthorized;

        private readonly string _clientMessage;

        public string ClientMessage => _clientMessage;
    }
}