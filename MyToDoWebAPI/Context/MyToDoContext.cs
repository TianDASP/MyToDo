using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using MyToDoWebAPI.Entities;

namespace MyToDoWebAPI.Context
{
    public class MyToDoContext : DbContext
    {
        //private readonly IHttpContextAccessor httpContextAccessor;

        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Memo> Memos { get; set; }
        public DbSet<User> Users { get; set; }
        //public int? CurrentUserId { get; set; }

        public MyToDoContext(DbContextOptions options ) :base(options)
        {
            //string id = "";
            //this.httpContextAccessor = httpContextAccessor;
            //if (httpContextAccessor.HttpContext == null)
            //{
            //    Console.WriteLine("注入http上下文为空");
            //}else if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x=>x.Type == "Id") == null)
            //{
            //    Console.WriteLine("上下文UserClaims 没有id字段");
            //}else
            //{
            //    id = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            //}
            //if (!string.IsNullOrEmpty(id))
            //{
            //    CurrentUserId = int.Parse(id); 
            //    Console.WriteLine($"当前用户:{CurrentUserId}");
            //}
            //else
            //{
            //    Console.WriteLine("获取id失败");
            //}
            //Console.WriteLine("dbctx初始化");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Console.WriteLine($"OnModelCreating:{CurrentUserId}");
            modelBuilder.Entity<ToDo>().HasOne<User>(x => x.Owner).WithMany(x=>x.ToDos).HasForeignKey(x=>x.OwnerId);
            modelBuilder.Entity<Memo>().HasOne<User>(x => x.Owner).WithMany(x => x.Memos).HasForeignKey(x => x.OwnerId);

            //if (CurrentUserId != null && CurrentUserId > 0)
            //{
            //    Console.WriteLine($"$应用过滤{CurrentUserId}");
            //    modelBuilder.Entity<ToDo>().HasQueryFilter(p=>p.OwnerId == CurrentUserId);
            //    modelBuilder.Entity<Memo>().HasQueryFilter(p=>p.OwnerId == CurrentUserId);
            //}
            base.OnModelCreating(modelBuilder);
        }
    }
}
