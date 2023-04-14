using System.ComponentModel.DataAnnotations;

namespace MyToDoWebAPI.Controllers
{
    public class RegisterRequest
    {        // 账号唯一名  字母 数字  _@*等符号
        [MaxLength(30)]
        public string Account { get; set; }

        // 昵称
        [MaxLength(20)]
        public string UserName { get; set; }
        // 未hash密码
        [MinLength(6)]
        [MaxLength(30)]
        public string Password { get; set; }
    }
}