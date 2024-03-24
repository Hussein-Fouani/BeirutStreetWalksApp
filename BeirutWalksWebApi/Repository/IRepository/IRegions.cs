using BeirutWalksDomains.Models;
using System.Linq.Expressions;

namespace BeirutWalksWebApi.Repository.IRepository
{
    public interface IRegions:IBaseRepository<Regions>
    {
        Task<Regions> UpdateAsync(Regions entity);
        Task<Regions> GetAsync(Expression<Func<Regions, bool>> filter = null);
        Task<List<Regions>> GetAllAsync(Expression<Func<Regions, bool>> filter = null);
    }
}
