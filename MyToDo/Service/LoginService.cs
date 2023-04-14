using MyToDo.Common.Models;
using MyToDo.Shared;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;

namespace MyToDo.Service
{
    public class LoginService : ILoginService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "Login";
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public LoginService(HttpRestClient client)
        {
            this.client = client;
        }
        public async Task<ApiResponse<LoginResponse>?> LoginAsync(UserDto dto)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Post;
            request.Route = $"api/{serviceName}/LoginByAccountAndPwd";
            request.Body = dto;
            var res = await client.ExecuteAsync(request);
            if (res?.StatusCode == System.Net.HttpStatusCode.OK)
            { 
                var token = res?.Headers?.Where(x=>x.Name == "token").Select(x=>x.Value).FirstOrDefault();
                //return JsonSerializer.Deserialize<ApiResponse>(res.Content, options);
                LoginResponse x = JsonSerializer.Deserialize<LoginResponse>(res.Content, options);
                return new ApiResponse<LoginResponse>(x) { Msg = (string)token};
            }
            else
            {
                // 可以写入日志
                return null;
            }
        }

        public async Task<ApiResponse?> RegisterAsync(RegisterRequest dto)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Post;
            request.Route = $"api/{serviceName}/Register";
            request.Body = dto; 
            var res = await client.ExecuteAsync(request);
            if (res?.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //return JsonSerializer.Deserialize<ApiResponse>(res.Content, options); 
                return new ApiResponse(res.Content);
            }
            else
            {
                // 可以写入日志
                return null;
            }
        }
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string UserName { get; set; } 
    }
}
