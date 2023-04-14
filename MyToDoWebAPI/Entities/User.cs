using Masuit.Tools.Security;
using Microsoft.Extensions.Hosting;
using MyToDoWebAPI.Controllers;
using System.ComponentModel.DataAnnotations;

namespace MyToDoWebAPI.Entities
{
    public class User : BaseEntity
    {
        // 账号唯一名  字母 数字  _@*等符号
        [MaxLength(30)]
        public string Account { get; set; }

        // 昵称
        [MaxLength(20)]
        public string UserName { get; set; }
        [StringLength(6)]
        public string Salt { get; private set; } = GenerateSalt();
        public string HashedPassword { get; private set; }

        public List<ToDo> ToDos { get; set; } 
        public List<Memo> Memos { get; set; }

        public static User Create(string account, string username, string password)
        {
            User user = new User();
            user.SetPassword(password);
            user.Account = account;
            user.UserName = username;
            return user;
        }

        private static string GenerateSalt()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void SetPassword(string password)
        {
            // 先sha256,再加盐,再sha256
            var hash1 = Encrypt.SHA256(password);
            var hash2 = Encrypt.SHA256(hash1 + Salt);
            this.HashedPassword = hash2;
        }

        public bool CheckPassword(string password)
        {
            // 先sha256,再加盐,再sha256
            var hash1 = Encrypt.SHA256(password);
            var hash2 = Encrypt.SHA256(hash1 + Salt);
            if (hash2 == HashedPassword)
            {
                return true;
            }
            return false;
        }
    }
}
