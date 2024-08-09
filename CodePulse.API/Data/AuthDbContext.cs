using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext :IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "c43d5512-4196-40a6-9093-cdc3f40b626c";
            var writerRoleId = "3112b9e1-4d5a-4e7e-9669-be38bf87d469";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };   

            builder.Entity<IdentityRole>().HasData(roles);

            // Create admin user
            var adminUserId = "80dec97e-a0f8-419d-8d9a-26e52eaa60d7";
            var admin = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedUserName = "admin@codepulse.com".ToUpper(),
                NormalizedEmail = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            
            builder.Entity<IdentityUser>().HasData(admin);

            // Gives roles  to admin
            var adminRoles = new List<IdentityUserRole<string>>
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId,
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId,
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    } 
}
