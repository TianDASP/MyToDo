using DryIoc;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Shared;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    /// <summary>
    /// 普通资源的 crud
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseService<TKey, TEntity> : IBaseService<TKey, TEntity>   where TEntity : BaseDto<TKey>
    {
        private readonly HttpRestClient client;
        private readonly string resourceName;
        //private readonly string token;
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        public BaseService(HttpRestClient client, string resourceName )
        {
            this.client = client; 
            this.resourceName = resourceName; 
        }
        // post单个资源
        public async Task<ApiResponse<TEntity>?> AddAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();

            request.JwtToken = AppSession.Token; 
            request.Method = Method.Post;
            request.Route = $"api/{resourceName}";
            request.Body = entity;
            var res = await client.ExecuteAsync(request);
            if(res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<ApiResponse<TEntity>>(res.Content, options);
            }
            else
            {
                // 可以写入日志
                return null;
            } 
        }

        public async Task<ApiResponse?> DeleteAsync(TKey id)
        {
            BaseRequest request = new BaseRequest();

            request.JwtToken = AppSession.Token;
            request.Method = Method.Delete;
            request.Route = $"api/{resourceName}/{id}";  
            var res = await client.ExecuteAsync(request);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<ApiResponse>(res.Content, options);
            }
            else
            {
                // 可以写入日志
                return null;
            }
        }

        public async Task<ApiResponse<IEnumerable<TEntity>>?> GetAllAsync()
        {
            BaseRequest request = new BaseRequest();
            request.JwtToken = AppSession.Token;
            request.Method = Method.Get;
            request.Route = $"api/{resourceName}";
            var res = await client.ExecuteAsync(request);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<ApiResponse<IEnumerable<TEntity>>>(res.Content, options);
            }
            else
            {
                // 可以写入日志
                return null;
            }
        }

        public async Task<ApiResponse<TEntity>?> GetFirstOrDefaultAsync(TKey id)
        {
            BaseRequest request = new BaseRequest();
            request.JwtToken = AppSession.Token;
            request.Method = Method.Get;
            request.Route = $"api/{resourceName}/{id}";
            var res = await client.ExecuteAsync(request);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<ApiResponse<TEntity>>(res.Content, options);
            }
            else
            {
                // 可以写入日志
                return null;
            }
        }

        public async Task<ApiResponse<TEntity>?> UpdateAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.JwtToken = AppSession.Token;
            request.Method = Method.Post;
            request.Route = $"api/{resourceName}/{entity.Id}";
            request.Body = entity;
            var res = await client.ExecuteAsync(request);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<ApiResponse<TEntity>>(res.Content, options);
            }
            else
            {
                // 可以写入日志
                return null;
            }
        }
    }
}
