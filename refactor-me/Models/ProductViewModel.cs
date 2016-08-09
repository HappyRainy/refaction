using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Services.Description;

namespace Xero.RefactoringExercise.WebApi.Models
{
    /// <summary>
    /// Product view model to be return to client side
    /// </summary>
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Product name is required.", AllowEmptyStrings = false), MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        //Need to set as nullable to make the Requried notation works for decimal
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Product delivery price is required.")]
        //Need to set as nullable to make the Requried notation works for decimal
        public decimal? DeliveryPrice { get; set; }
    }
}