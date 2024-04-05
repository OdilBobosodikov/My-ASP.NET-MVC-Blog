using Microsoft.EntityFrameworkCore;
using MyBlog.web.Data;
using MyBlog.web.Models.Domain;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Repositories.Classes
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly MyBlogDbContext blogDbContext;

        public BlogPostRepository(MyBlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await blogDbContext.AddAsync(blogPost);
            await blogDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
           var existingBlog = await blogDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null)
            {
                blogDbContext.BlogPosts.Remove(existingBlog);
                await blogDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;

        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await blogDbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<BlogPost?> GetBySlugAsync(string slug)
        {
           return await blogDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.Slug == slug);

        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
          var oldBlog = await blogDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=> x.Id == blogPost.Id);

            if(oldBlog != null) 
            {
                oldBlog.Id = blogPost.Id;
                oldBlog.Heading = blogPost.Heading;
                oldBlog.Title = blogPost.Title;
                oldBlog.Content = blogPost.Content;
                oldBlog.ShortDescription = blogPost.ShortDescription;
                oldBlog.Author = blogPost.Author;
                oldBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                oldBlog.Slug = blogPost.Slug;
                oldBlog.Visible = blogPost.Visible;
                oldBlog.PublishedDate = blogPost.PublishedDate;
                oldBlog.Tags = blogPost.Tags;

                await blogDbContext.SaveChangesAsync();
                return oldBlog;
            }
            return null;
        }
    }
}
