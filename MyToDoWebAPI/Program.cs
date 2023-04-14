using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyToDoWebAPI.Context;
using System.Text;

namespace MyToDoWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<MyToDoContext>(option =>
            {
                option.UseSqlite(builder.Configuration.GetConnectionString("ToDoConnection"));
            } );
            //builder.Services.AddDbContext<IdentityDbContext>(option =>
            //{
            //    option.UseSqlite(builder.Configuration.GetConnectionString("ToDoConnection"));
            //}, ServiceLifetime.Scoped);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddHttpContextAccessor();
            #region 添加jwt鉴权服务：
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ClockSkew = TimeSpan.FromSeconds(30), // 多久验证一次
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = "http://localhost:5211",//Audience
                        ValidIssuer = "http://localhost:5211",//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ASSD-SDFS-ASDW-RTJR-XCVS-RTYR"))//拿到解密的SecurityKey/密钥
                    };
                });
            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //添加鉴权到管道
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}