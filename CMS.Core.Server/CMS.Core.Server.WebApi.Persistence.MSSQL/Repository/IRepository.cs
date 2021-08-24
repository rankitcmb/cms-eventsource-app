using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Core.Server.WebApi.Persistence.MSSQL.Repository
{
    public interface IRepository<T,TKey> where T: class where TKey : struct
    {
        Task<T> GetByIdAsync(TKey id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<TKey> AddAsync(T entity);
        Task<TKey> UpdateAsync(T entity);
        Task<TKey> DeleteAsync(TKey id);
    }
}