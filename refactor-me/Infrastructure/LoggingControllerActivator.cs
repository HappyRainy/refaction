using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using NLog;

namespace Xero.RefactoringExercise.WebApi.Infrastructure
{
    /// <summary>
    /// Wraps a a controller activator to add sensible logging of failures when trying to create
    /// instances of controllers. This makes debugging Ninject errors such as not having dependencies
    /// registered much easier.
    /// </summary>
    public class LoggingControllerActivator : IHttpControllerActivator
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        readonly IHttpControllerActivator _instance;

        public LoggingControllerActivator(IHttpControllerActivator instance)
        {
            _instance = instance;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            try
            {
                return _instance.Create(request, controllerDescriptor, controllerType);
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, $"Controller activator failed to create an instance of {controllerDescriptor.ControllerType.Name}.", ex);
                throw;
            }
        }
    }
}