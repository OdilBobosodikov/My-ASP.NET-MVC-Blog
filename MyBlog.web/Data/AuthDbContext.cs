using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "2ba76584-697e-40ea-9240-cdb5808454c4";
            var userRoleId = "f5ffc062-4ea1-450f-bdb4-06cc2ac36c3d";
            var superAdminRoleId = "947fd026-535d-4430-a522-b65da60904c8";
            
            var roles = new List<IdentityRole> {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },

                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                },

                new IdentityRole
                {
                    Name = "SuperUser",
                    NormalizedName = "SuperUser",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            var superAdminId = "eba54ce2-8e95-4d34-b67a-f40079a9e03f"; 
           
            var superAdminUser = new IdentityUser()
            {
                UserName = "superadmin",
                Email = "bobosodikov0d@gmail.com",
                NormalizedEmail = "bobosodikov0d@gmail.com".ToUpper(),
                NormalizedUserName = "superadmin".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Qtunisdeadnow_1");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId =  superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId =  superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId =  superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
