using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    public class RegisterRequest : BindableBase
    {
        [MaxLength(30)]
        private string account;
        public string Account { get=>account; set=>SetProperty(ref account, value); }

        // 昵称
        [MaxLength(20)]
        private string userName;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        // 未hash密码
        [MinLength(6)]
        [MaxLength(30)]
        private string password;
        public string Password { get => password; set => SetProperty(ref password, value); } 
    }
}
