using Microsoft.EntityFrameworkCore;
using MyBlog.web.Data;
using MyBlog.web.Models.Domain;
using MyBlog.web.Models.ViewModels;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Repositories.Classes
{
    public class TagRepository : ITagRepository
    {
        private readonly MyBlogDbContext dbContext;
        public TagRepository(MyBlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<Tag> AddAsync(Tag tag)
        {
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tagToDelete = await dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tagToDelete != null) 
            {
                dbContext.Tags.Remove(tagToDelete);
                await dbContext.SaveChangesAsync();

                return tagToDelete;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageNum = 1,
            int pageSize = 100)
        {
            var query = dbContext.Tags.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.Name.Contains(searchQuery) || 
                                         x.DisplatyName.Contains(searchQuery));
            }

            var skipResults = (pageNum - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x=>x.Name);
                }
                else if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplatyName) : query.OrderBy(x => x.DisplatyName);
                }
            }


            return await query.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await dbContext.Tags.FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var oldTag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Id == tag.Id);

            if (oldTag != null)
            {
                oldTag.Name = tag.Name;
                oldTag.DisplatyName = tag.DisplatyName;

                await dbContext.SaveChangesAsync();

                return oldTag;
            }
            return null;
        }


        public async Task<int> CountAsync()
        {
            return await dbContext.Tags.CountAsync();
        }
    }
}
