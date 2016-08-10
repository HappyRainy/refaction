using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Xero.RefactoringExercise.DAL.Supports
{
    /// <summary>
    /// Abstract class entity is the base class for all db entity classes
    /// </summary>
    public abstract class Entity: IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedOn { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(128)]
        public string CreatedBy { get; set; }

        [MaxLength(128)]
        public string UpdatedBy { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
