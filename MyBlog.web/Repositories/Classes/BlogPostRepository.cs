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

        public async Task<int> CountAsync(string? tagName = null)
        {
            if (tagName != null)
            {
                return await GetQueryableByTagName(tagName).CountAsync();
            }
            return await blogDbContext.BlogPosts.CountAsync();
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

        public async Task<IEnumerable<BlogPost>> GetAllAsync(string? searchQuery = null,
                                                             string? sortBy = null,
                                                             string? sortDirection = null,
                                                             int pageNumber = 1,
                                                             int pageSize = 100,
                                                             string? tagName = null)
        {
            var query = GetQueryableByTagName(tagName);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.Heading.Contains(searchQuery) ||
                                         x.Author.Contains(searchQuery) ||
                                         x.Title.Contains(searchQuery) ||
                                         x.ShortDescription.Contains(searchQuery));
            }

            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            if (!string.IsNullOrWhiteSpace(sortBy)) 
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Heading", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Heading) : query.OrderBy(x => x.Heading);
                }
            }

            return await query.Include(x => x.Tags).ToListAsync();
        }

        private IQueryable<BlogPost> GetQueryableByTagName(string? tagName)
        {
            var query = blogDbContext.BlogPosts.AsQueryable();

            if (!string.IsNullOrEmpty(tagName))
            {
                var tag = blogDbContext.Tags.FirstOrDefault(x => x.Name == tagName);
                if (tag != null)
                {
                    query = query.Where(x => x.Tags.Contains(tag));
                }
            }
            return query;
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
