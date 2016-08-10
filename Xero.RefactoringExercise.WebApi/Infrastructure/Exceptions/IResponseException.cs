using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Xero.RefactoringExercise.WebApi.Infrastructure.Exceptions
{
    /// <summary>
    /// Response excpetion interface, all exception raised by system should implement this interface
    /// </summary>
    public interface IResponseException
    {
        /// <summary>
        /// The HTTP status code to set on the web response. 
        /// </summary>
        HttpStatusCode Code { get; }

        /// <summary>
        /// Message to give more info to client
        /// </summary>
        string ClientMessage { get; }
    }
}
