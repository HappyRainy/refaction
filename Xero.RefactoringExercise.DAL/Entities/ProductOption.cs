using System;
using System.ComponentModel.DataAnnotations;
using Xero.RefactoringExercise.DAL.Supports;

namespace Xero.RefactoringExercise.DAL.Entities
{
    public class ProductOption : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
