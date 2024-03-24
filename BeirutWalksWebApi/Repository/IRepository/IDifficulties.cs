using BeirutWalksDomains.Models;
using System.Linq.Expressions;

namespace BeirutWalksWebApi.Repository.IRepository
{
    public interface IDifficulties:IBaseRepository<Difficulty>
    {
        Task<Difficulty> UpdateAsync(Difficulty entity);
        Task<Difficulty> GetAsync(Expression<Func<Difficulty, bool>> filter = null);
        Task<List<Difficulty>> GetAllAsync(Expression<Func<Difficulty, bool>> filter = null);
    }
}
