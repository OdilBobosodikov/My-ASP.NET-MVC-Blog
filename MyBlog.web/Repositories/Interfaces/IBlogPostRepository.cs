using MyBlog.web.Models.Domain;

namespace MyBlog.web.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync(string? searchQuery = null, 
                                                string? sortBy = null,
                                                string? sortDirection = null,
                                                int pageNumber = 1,
                                                int pageSize = 100);
        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost?> GetBySlugAsync(string slug);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid id);

        Task<int> CountAsync();
        Task<IEnumerable<BlogPost>> GetAllByTagName(string? tagName);
    }
}
