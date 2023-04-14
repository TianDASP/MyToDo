using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyToDoWebAPI.Dtos
{
    public class ToDoDto
    { 
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
        // 完成为True 未完成为False
        public bool Status { get; set; }
        [AllowNull]
        public DateTime CreateDate { get; set; }  

        [AllowNull]
        public DateTime UpdateTime { get; set; }

    }
}
