using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyToDoWebAPI.Entities
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [AllowNull]
        public DateTime UpdateTime { get; set; }

        [AllowNull]
        public DateTime DeleteTime { get; set; } 
    }
}
