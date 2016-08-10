using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xero.RefactoringExercise.DAL.Supports;

namespace Xero.RefactoringExercise.DAL.Entities
{
    [Table("ProductOption")]
    public class ProductOption : Entity
    {
        [Index("IX_UNIQUE_ProductId_ProductOptionName", 1, IsUnique = true)]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [MaxLength(100)]
        [Index("IX_UNIQUE_ProductId_ProductOptionName", 2, IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
