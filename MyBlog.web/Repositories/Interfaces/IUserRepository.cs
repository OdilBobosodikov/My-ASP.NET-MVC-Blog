using Microsoft.AspNetCore.Identity;

namespace MyBlog.web.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
