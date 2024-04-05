using MyBlog.web.Models.Domain;

namespace MyBlog.web.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(string? searchValue = null,
            string? sortBy = null,
            string? sortDirection = null,
            int pageNum = 1,
            int pageSize = 100);
        Task<Tag?> GetAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tag);
        Task<Tag?> DeleteAsync(Guid id);

        Task<int> CountAsync();

    }
}
