using MyBlog.web.Models.Domain;

namespace MyBlog.web.Repositories.Interfaces
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);

        Task<IEnumerable<BlogPostComment>> GetAllByIdAsync(Guid blogPostId);
    }
}
