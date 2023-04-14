using MyToDoWebAPI.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MyToDoWebAPI.Entities
{
    /// <summary>
    /// 备忘录
    /// </summary>
    public class Memo : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
        public int OwnerId { get; set; }
        public User? Owner { get; set; }

        public static Memo Create(string title, string content,User? user)
        {
            Memo memo = new Memo();
            memo.Title = title;
            memo.Content = content; 
            memo.Owner =  user ?? null;
            return memo;
        }

        public void Update(MemoDto dto)
        {
            this.Title = dto.Title;
            this.Content = dto.Content; 
            this.UpdateTime = DateTime.Now;
        }
    }
}
