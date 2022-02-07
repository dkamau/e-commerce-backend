using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceBackend.Core.Interfaces
{
    public interface ICrudService<T>
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<T> GetBySlugAsync(string slug);
        Task<List<T>> ListAsync(object filter);
        Task<int> CountAsync(object filter);
        Task<T> SoftDeleteAsync(int id);
        Task<bool> RecordExistsAsync(int id);
        Task DeleteAsync(int accountId);
    }
}
