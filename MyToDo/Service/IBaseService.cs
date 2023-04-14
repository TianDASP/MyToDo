using MyToDo.Shared;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IBaseService<TKey,TEntity> where TEntity : class
    {
        Task<ApiResponse<TEntity>?> AddAsync(TEntity entity);
        Task<ApiResponse<TEntity>?> UpdateAsync(TEntity entity);
        Task<ApiResponse?> DeleteAsync(TKey id);
        Task<ApiResponse<TEntity>?> GetFirstOrDefaultAsync(TKey id);
        //Task<ApiResponse<PagedList<TEntity>> GetAllAsync(QueryParameter parameter;)
        Task<ApiResponse<IEnumerable<TEntity>>?> GetAllAsync();  
    }
}
