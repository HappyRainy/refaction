using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xero.RefactoringExercise.DAL.Supports;

namespace Xero.RefactoringExercise.DAL.Entities
{
    public class Product : Entity<Guid>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public ICollection<ProductOption> ProductOptions { get; set; }
    }
}
