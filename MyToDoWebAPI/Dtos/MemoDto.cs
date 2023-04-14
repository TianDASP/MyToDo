using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyToDoWebAPI.Dtos
{
    public class MemoDto
    { 
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
        [AllowNull]
        public DateTime CreateDate { get; set; } 

        [AllowNull]
        public DateTime UpdateTime { get; set; }
    }
}
