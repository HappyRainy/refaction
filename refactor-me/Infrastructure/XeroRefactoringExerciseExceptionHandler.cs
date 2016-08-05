using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace Xero.RefactoringExercise.WebApi.Infrastructure
{
    public class XeroRefactoringExerciseExceptionHandler : ExceptionHandler
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }

        public override async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            // We expect the framework to always supply at least an exception and a request, all
            // other properties are optional and should be null checked.
            Contract.Assert(context.Exception != null);
            Contract.Assert(context.Request != null);

            var contentNegotiator = context.RequestContext.Configuration.Services.GetContentNegotiator();
            var formatters = context.RequestContext.Configuration.Formatters;


            _log.Info("Exception handler hit with exception type '{0}'.", context.Exception.GetType());

            HttpStatusCode code;

            _log.Fatal(context.Exception, "Unhandled exception caught.");

            code = HttpStatusCode.InternalServerError;

            // When an exception occurs we want to log request body to aid in resolving issues
            await LogRequestBody(context.Request);

            context.Result = context.Result = new TextPlainErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = "Oops! Sorry! Something went wrong." +
                          "Please contact lijingch@gmail.com. I can fix for you."
            };
        }


        static async Task LogRequestBody(HttpRequestMessage request)
        {
            var requestBody =
                (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
                    && !request.RequestUri.AbsolutePath.Contains("login")
                    ? await request.Content.ReadAsStringAsync()
                    : null;

            if (requestBody != null)
                _log.Info(new {RequestBody = requestBody});
        }

        private class TextPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response =
                                 new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(Content);
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }
    }
}