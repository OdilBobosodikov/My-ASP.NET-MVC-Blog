using Microsoft.EntityFrameworkCore;
using MyBlog.web.Data;
using MyBlog.web.Models.Domain;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Repositories.Classes
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly MyBlogDbContext myBlogDbContext;

        public BlogPostCommentRepository(MyBlogDbContext myBlogDbContext)
        {
            this.myBlogDbContext = myBlogDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await myBlogDbContext.BlogPostComments.AddAsync(blogPostComment);
            await myBlogDbContext.SaveChangesAsync();

            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetAllByIdAsync(Guid blogPostId)
        {
            var query = myBlogDbContext.BlogPostComments
                .Where(x => x.BlogPostId == blogPostId)
                .AsQueryable()
                .OrderByDescending(x => x.DateAdded);

            return await query.ToListAsync();
        }
    }
}
