using System;
using System.Collections.Generic;
using System.Linq;
using System.Net; 
using System.Threading.Tasks;

namespace MyToDo.Shared
{
    public class ApiResponse
    {
        public ApiResponse()
        {

        }
        public ApiResponse(object content)
        {
            Code = 200;
            Content = content;
        }
        // http状态码加 1000开始的自定义错误码
        public int Code { get; set; } = 200;
        // 放错误信息,可以在测试时使用, 发布去除
        public string Msg { get; set; } = "";
        public object Content { get; set; }
    }
    public class ApiResponse<TEntity> where TEntity : class
    {
        // http状态码加 1000开始的自定义错误码
        public ApiResponse()
        {

        }
        public ApiResponse(TEntity content)
        {
            Code = 200;
            Content = content;
        }
        public int Code { get; set; } = 0;
        // 放错误信息,可以在测试时使用
        public string Msg { get; set; } = "";
        public TEntity Content { get; set; }  
    } 
}
