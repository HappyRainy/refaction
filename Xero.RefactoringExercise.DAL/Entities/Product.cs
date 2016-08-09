using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xero.RefactoringExercise.DAL.Supports;

namespace Xero.RefactoringExercise.DAL.Entities
{
    public class Product : Entity
    {
        [Required]
        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal DeliveryPrice { get; set; }

        [InverseProperty("Product")]
        public ICollection<ProductOption> ProductOptions { get; set; }
    }
}
