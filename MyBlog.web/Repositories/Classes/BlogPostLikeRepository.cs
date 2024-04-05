using Microsoft.EntityFrameworkCore;
using MyBlog.web.Data;
using MyBlog.web.Models.Domain;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Repositories.Classes
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly MyBlogDbContext blogDbContext;

        public BlogPostLikeRepository(MyBlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await blogDbContext.BlogPostLikes.AddAsync(blogPostLike);
            await blogDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<BlogPostLike?> DeleteLike(BlogPostLike blogPostLike)
        {
            if (blogPostLike != null)
            {
                blogDbContext.BlogPostLikes.Remove(blogPostLike);
                await blogDbContext.SaveChangesAsync();
                return blogPostLike;
            }
            return null;
        }

        public async Task<BlogPostLike?> GetLike(Guid blogPostId)
        {
            return await blogDbContext.BlogPostLikes.FirstOrDefaultAsync(x => x.BlogPostId == blogPostId);
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await blogDbContext.BlogPostLikes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid id)
        {
            return await blogDbContext.BlogPostLikes.CountAsync(x => x.BlogPostId == id);
        }
    }
}
