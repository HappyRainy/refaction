using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Xero.RefactoringExercise.DAL.Supports
{
    /// <summary>
    /// Abstract class entity is the base class for all db entity classes
    /// </summary>
    public abstract class Entity<T> : IEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

        object IEntity.Id {
            get { return this.Id; }
            set { throw new InvalidOperationException("Unable to set entity id through explicit IEntity.Id set implementation "); }
        }

        private DateTime? _createdOn;
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn
        {
            get { return _createdOn ?? DateTime.UtcNow; }
            set { _createdOn = value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedOn { get; set; }

        [MinLength(1), MaxLength(128)]
        public string CreatedBy { get; set; }

        [MinLength(1), MaxLength(128)]
        public string UpdatedBy { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }


}
