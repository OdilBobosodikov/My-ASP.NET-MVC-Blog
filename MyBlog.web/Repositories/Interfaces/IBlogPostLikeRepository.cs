using MyBlog.web.Models.Domain;

namespace MyBlog.web.Repositories.Interfaces
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid id);
        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);
        Task<BlogPostLike?> GetLike(Guid blogPostId);
        Task<BlogPostLike?> DeleteLike(BlogPostLike blogPostLike);
    }
}
