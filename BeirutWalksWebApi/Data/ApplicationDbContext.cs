using BeirutWalksDomains.Models;
using Microsoft.EntityFrameworkCore;

namespace BeirutWalksWebApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt):base(opt)
        {

            
            
        }
        public DbSet<Difficulty> Difficulty { get; set; }
        public DbSet<Walks> Walks { get; set; }
        public DbSet<Regions> Regions { get; set; }
        public DbSet<Image> Images { get; set; }   

       

    }
}
