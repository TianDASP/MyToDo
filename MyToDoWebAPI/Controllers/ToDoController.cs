using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyToDo.Shared;
using MyToDoWebAPI.Context;
using MyToDoWebAPI.Dtos;
using MyToDoWebAPI.Entities;

namespace MyToDoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    { 
        private readonly MyToDoContext dbctx;
        private readonly IMapper mapper;
        private User? user;
        public ToDoController(IHttpContextAccessor httpContextAccessor,MyToDoContext dbctx, IMapper mapper)
        {
            this.dbctx = dbctx;
            this.mapper = mapper; 
            var idStr = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            GetUserAsync(idStr);
            Console.WriteLine($"当前用户:{idStr}");
        }

        private async void GetUserAsync(string? idStr)
        {
            if (!string.IsNullOrEmpty(idStr))
            {
                var id = int.Parse(idStr);
                // 这里使用同步方法查找用户
                user =await dbctx.Users.FirstAsync(x => x.Id == id);
            } 
        }
        [HttpGet]
        public async Task<IActionResult> GetToDos()
        {
            var todos = await dbctx.ToDos.Where(x=>x.OwnerId == user.Id).ToListAsync();
            var dtosToReturn = todos.Select(todo => mapper.Map<ToDoDto>(todo));
            return Ok(new ApiResponse(dtosToReturn));
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> UpdateToDo(int id, [FromBody] ToDoDto model)
        {
            var todoToUpdate = await dbctx.ToDos.Where(x => x.OwnerId == user.Id).FirstOrDefaultAsync(x => x.Id == id);
            if (todoToUpdate == null)
            {
                return BadRequest("更新失败,请检查请求");
            }
            todoToUpdate.Update(model);
            dbctx.Update(todoToUpdate);
            var numAdded = await dbctx.SaveChangesAsync();
            if (numAdded > 0)
            {
                var dtoToReturn = mapper.Map<ToDoDto>(todoToUpdate);
                return Ok(new ApiResponse(dtoToReturn));
            }
            return BadRequest("更新失败,请联系管理员");
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo([FromBody] ToDoDto model)
        { 
            // string userName = httpContextAccessor.HttpContext.User.Identity.Name;
            //int userId;
            //User? user = null;
            //int.TryParse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value, out userId);
            //if (userId > 0)
            //{
            //    user = await dbctx.Users.FirstOrDefaultAsync(x => x.Id == userId);
            //}

            // 这里赋值Owner可以延后到DBctx.SaveChanges,在那里统一处理
            var todoToAdd = ToDo.Create(model.Title, model.Content, model.Status,user);
            await dbctx.AddAsync(todoToAdd);
            var addRes = await dbctx.SaveChangesAsync();
            if (addRes > 0)
            {
                var dtoToReturn = mapper.Map<ToDoDto>(todoToAdd);
                return Ok(new ApiResponse(dtoToReturn));
            }
            return BadRequest("更新失败,请联系管理员");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetToDo(int id)
        {
            var toDo = await dbctx.ToDos.Where(x => x.OwnerId == user.Id).FirstOrDefaultAsync(x => x.Id == id);
            if (toDo == null)
            {
                return BadRequest("获取数据失败,请检查请求");
            }
            // mapper 
            var toDoDto = mapper.Map<ToDoDto>(toDo);
            return Ok(new ApiResponse(toDoDto));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var toDo = await dbctx.ToDos.Where(x => x.OwnerId == user.Id).FirstOrDefaultAsync(x => x.Id == id);
            if (toDo == null)
            {
                return BadRequest("删除失败,请检查请求");
            }
            dbctx.Remove(toDo);
            await dbctx.SaveChangesAsync();
            return Ok(new ApiResponse() { Code = 200 });
        }
    }
}
