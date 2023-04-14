using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyToDoWebAPI.Context;
using MyToDoWebAPI.Entities;
using MyToDoWebAPI.Dtos;
using MyToDo.Shared;
using Microsoft.AspNetCore.Authorization;

namespace MyMemoWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemoController : ControllerBase
    { 
        private readonly MyToDoContext dbctx;
        private readonly IMapper mapper;
        private User? user;
        public MemoController(IHttpContextAccessor httpContextAccessor, MyToDoContext dbctx, IMapper mapper)
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
                user = await dbctx.Users.FirstAsync(x => x.Id == id);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetMemos()
        {
            var memos = await dbctx.Memos.Where(x => x.OwnerId == user.Id).ToListAsync();
            var dtosToReturn = memos.Select(todo => mapper.Map<MemoDto>(todo));
            return Ok(new ApiResponse(dtosToReturn));
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> UpdateMemo(int id, [FromBody] MemoDto model)
        {
            var MemoToUpdate = await dbctx.Memos.Where(x => x.OwnerId == user.Id).FirstOrDefaultAsync(x => x.Id == id);
            if (MemoToUpdate == null)
            {
                return BadRequest("更新失败,请检查请求");
            }
            MemoToUpdate.Update(model);
            dbctx.Update(MemoToUpdate);
            var numAdded = await dbctx.SaveChangesAsync();
            if (numAdded > 0)
            {
                var MemoToReturn = mapper.Map<MemoDto>(MemoToUpdate);
                return Ok(new ApiResponse(MemoToReturn));
            }
            return BadRequest("更新失败,请联系管理员");
        }

        [HttpPost]
        public async Task<IActionResult> AddMemo([FromBody] MemoDto model)
        { 
            var MemoToAdd = Memo.Create(model.Title, model.Content, user);

            await dbctx.AddAsync(MemoToAdd);
            var addRes = await dbctx.SaveChangesAsync();
            if (addRes > 0)
            {
                var MemoToReturn = mapper.Map<MemoDto>(MemoToAdd);
                return Ok(new ApiResponse(MemoToReturn));
            }
            return BadRequest("更新失败,请联系管理员");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMemo(int id)
        {
            var Memo = await dbctx.Memos.Where(x => x.OwnerId == user.Id).FirstOrDefaultAsync(x => x.Id == id);
            if (Memo == null)
            {
                return BadRequest("获取数据失败,请检查请求");
            }
            // mapper 
            var MemoDto = mapper.Map<MemoDto>(Memo);
            return Ok(new ApiResponse() { Content = MemoDto });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMemo(int id)
        {
            var memo = await dbctx.Memos.Where(x => x.OwnerId == user.Id).FirstOrDefaultAsync(x => x.Id == id);
            if (memo == null)
            {
                return BadRequest("删除失败,请检查请求");
            }
            dbctx.Remove(memo);
            await dbctx.SaveChangesAsync();
            return Ok(new ApiResponse() { Code = 200 });
        }
    }
}
