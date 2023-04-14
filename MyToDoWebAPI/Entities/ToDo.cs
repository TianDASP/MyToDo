using MyToDoWebAPI.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MyToDoWebAPI.Entities
{
    /// <summary>
    /// 待办
    /// </summary>
    public class ToDo : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
        // 完成为True 未完成为False
        public bool Status { get; set; } = false;

        public int OwnerId { get; set; }
        public User? Owner { get; set; }

        public static ToDo Create(string title, string content, bool status, User? user)
        {
            ToDo todo = new ToDo();  
            todo.Title = title;
            todo.Content = content;
            todo.Status = status;
            todo.Owner = user ?? null;
            return todo;
        }

        public void Update(ToDoDto dto)
        {
            this.Title = dto.Title;
            this.Content = dto.Content;
            this.Status = dto.Status;
            this.UpdateTime = DateTime.Now;
        }
    }
}
