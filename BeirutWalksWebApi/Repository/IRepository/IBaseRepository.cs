using System.Linq.Expressions;

namespace BeirutWalksWebApi.Repository.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        
        Task SaveAsync();
    }
}
