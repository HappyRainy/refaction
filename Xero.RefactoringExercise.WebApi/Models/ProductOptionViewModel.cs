using System;
using System.ComponentModel.DataAnnotations;

namespace Xero.RefactoringExercise.WebApi.Models
{
    /// <summary>
    /// Product option view model to be return to client side
    /// </summary>
    public class ProductOptionViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Product option name is required.", AllowEmptyStrings = false), MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

    }
}