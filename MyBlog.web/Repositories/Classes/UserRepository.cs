using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.web.Data;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Repositories.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;
        private readonly string superAdminEmail = "bobosodikov0d@gmail.com";

        public UserRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await authDbContext.Users.ToListAsync();

            var superadmin = await authDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == superAdminEmail);

            if (superadmin != null)
            {
                users.Remove(superadmin);
            }

            return users;
        }
    }
}
