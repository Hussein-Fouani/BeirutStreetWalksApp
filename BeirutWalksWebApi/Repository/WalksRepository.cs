 using AutoMapper;
using BeirutWalksDomains.Models;
using BeirutWalksWebApi.Data;
using BeirutWalksWebApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BeirutWalksWebApi.Repository
{
    public class WalksRepository : BaseRepository<Walks>, IWalks
    {
        private readonly ApplicationDbContext db;

        public WalksRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task<Walks> UpdateAsync(Walks entity)
        {
            db.Walks.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }
        public async Task<List<Walks>> GetAllAsync(Expression<Func<Walks, bool>> filter = null)
        {
            IQueryable<Walks> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.Include("Regions").Include("Difficulty").ToListAsync();
        }

        public async Task<Walks> GetAsync(Expression<Func<Walks, bool>> filter = null)
        {
            IQueryable<Walks> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.Include("Regions").Include("Difficulty").FirstOrDefaultAsync();
        }

    }

}
