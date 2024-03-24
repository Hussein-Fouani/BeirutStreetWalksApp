using BeirutWalksDomains.Models;
using BeirutWalksWebApi.Data;
using BeirutWalksWebApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BeirutWalksWebApi.Repository
{
    public class RegionRepository : BaseRepository<Regions>, IRegions
    {
        private readonly ApplicationDbContext db;

        public RegionRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task<Regions> UpdateAsync(Regions entity)
        {
            db.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }
        public async Task<List<Regions>> GetAllAsync(Expression<Func<Regions, bool>> filter = null)
        {
            IQueryable<Regions> query = (IQueryable<Regions>)db;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Regions> GetAsync(Expression<Func<Regions, bool>> filter = null)
        {
            IQueryable<Regions> query = (IQueryable<Regions>)db;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
