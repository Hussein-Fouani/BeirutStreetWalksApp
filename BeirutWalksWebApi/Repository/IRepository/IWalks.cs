using BeirutWalksDomains.Models;
using System.Linq.Expressions;

namespace BeirutWalksWebApi.Repository.IRepository
{
    public interface IWalks:IBaseRepository<Walks>
    {
        Task<Walks> GetAsync(Expression<Func<Walks, bool>> filter = null);
        Task<List<Walks>> GetAllAsync(Expression<Func<Walks, bool>> filter = null);
        Task<Walks> UpdateAsync(Walks entity);
    }
}
