using MyToDo.Common.Models;
using MyToDo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface ILoginService
    {
        Task<ApiResponse<LoginResponse>?> LoginAsync(UserDto dto);
        Task<ApiResponse?> RegisterAsync(RegisterRequest dto);
    }
}
 