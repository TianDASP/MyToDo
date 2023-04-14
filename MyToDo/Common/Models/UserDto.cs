using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    public class UserDto
    {
        [NotNull]
        [MaxLength(30)]
        public string Account { get; set; }
        [NotNull]
        [MinLength(6)]
        [MaxLength(30)]
        public string Password { get; set; }
    }
}
