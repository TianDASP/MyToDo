using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MyToDoWebAPI.Controllers
{
    public class LoginRequest
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