using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeirutWalksWebApi.Data
{
    public class AuthDb : IdentityDbContext
    {
        public AuthDb(DbContextOptions<AuthDb> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "3ed717a7-4555-4bc9-b7f8-97c493ec2cbb";
            var writerRoleId = "00438ce5-a855-49a9-be98-0353c8dfd68b";
            var roles = new List<IdentityRole>
            {
               new IdentityRole{Id=readerRoleId,ConcurrencyStamp=readerRoleId, Name = "Reader", NormalizedName = "Reader".ToUpper()},
                new IdentityRole{Id=writerRoleId,ConcurrencyStamp=writerRoleId,Name = "Writer", NormalizedName = "Writer".ToUpper()}
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
