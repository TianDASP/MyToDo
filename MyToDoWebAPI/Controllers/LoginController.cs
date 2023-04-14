using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyToDoWebAPI.Context;
using MyToDoWebAPI.Dtos;
using MyToDoWebAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyToDoWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController] 
    public class LoginController : ControllerBase
    {
        private readonly MyToDoContext dbctx;
        private readonly IMapper mapper;

        public LoginController(MyToDoContext dbctx, IMapper mapper)
        {
            this.dbctx = dbctx;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            var res = await dbctx.Users.FirstOrDefaultAsync(x => x.Account == request.Account);
            if (res != null)
            {
                return BadRequest("账号已存在");
            }
            var userToAdd = MyToDoWebAPI.Entities.User.Create(request.Account, request.UserName, request.Password);
            await dbctx.AddAsync(userToAdd);
            var numAdded =  await dbctx.SaveChangesAsync();
            if (numAdded == 0)
            {
                return BadRequest("注册账号失败,请稍后重试");
            }
            var dtoToReturn  = mapper.Map<UserDto>(userToAdd);
            return Ok(dtoToReturn);
        }
         
        [HttpPost]
        public async Task<IActionResult> LoginByAccountAndPwd([FromBody]LoginRequest account )
        {
            // 搜索账号
            var res = await dbctx.Users.FirstOrDefaultAsync(x => x.Account == account.Account);
            if (res == null)
            {
                return BadRequest("登录失败,请检查账号与密码");
            } 
             // 检查密码
            if (res.CheckPassword(account.Password))
            {
                var user = await dbctx.Users.FirstOrDefaultAsync(x => x.Account == account.Account);
                var dtoToReturn = mapper.Map<UserDto>(user);
                // 登录成功
                var claims = new Claim[]
                { 
                    new Claim(ClaimTypes.Name, user.Account),
                    new Claim("Id", user.Id.ToString()),
                    //new Claim("UserName", author.Name), 
                };
                // 密钥
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ASSD-SDFS-ASDW-RTJR-XCVS-RTYR"));
                // issuer代表颁发Token的Web Application(授权服务器),audience是Token的受理者(true 服务器)
                var token = new JwtSecurityToken(
                    issuer: "http://localhost:5211",
                    audience: "http://localhost:5211",
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1), //1hour后过期
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256) //
                    );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                Response.Headers.Add("token", jwtToken);  
                // 返回
                return Ok(new { user.Id, user.Account, user.UserName });
            }
            return BadRequest("登录失败,请检查账号与密码");
        }
    }
}
