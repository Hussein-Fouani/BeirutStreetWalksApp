using AutoMapper;
using BeirutWalksWebApi.Data;
using BeirutWalksWebApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BeirutWalksWebApi.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;
        internal DbSet<T> dbSet;

        public BaseRepository(ApplicationDbContext db)
        {
            this.db = db;
            this.dbSet = db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
           await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(T entity)
        {
             dbSet.Remove(entity);
            await SaveAsync();
        }

      
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
