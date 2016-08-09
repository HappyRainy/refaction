using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;

namespace Xero.RefactoringExercise.Tests.Support
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Help to validate view model through controller
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TController"></typeparam>
        /// <param name="controller"></param>
        /// <param name="viewModelToValidate"></param>
        public static void ValidateViewModel<TViewModel, TController>(this TController controller, TViewModel viewModelToValidate)
            where TController : ApiController
        {
            var validationContext = new ValidationContext(viewModelToValidate, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModelToValidate, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.FirstOrDefault() ?? string.Empty, validationResult.ErrorMessage);
            }
        }
    }
}
