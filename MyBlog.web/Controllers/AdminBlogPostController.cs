using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlog.web.Data;
using MyBlog.web.Models.Domain;
using MyBlog.web.Models.ViewModels;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest()
            {
                PublishedDate = DateTime.Now,
                Tags = tags.Select(x=> new SelectListItem {Text = x.Name, Value=x.Id.ToString() }),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest blogPostRequest)
        {
            var blogPost = new BlogPost()
            {
                Heading = blogPostRequest.Heading,
                Title = blogPostRequest.Title,
                Content = blogPostRequest.Content,
                ShortDescription = blogPostRequest.ShortDescription,
                FeaturedImageUrl = blogPostRequest.FeaturedImageUrl,
                Slug = blogPostRequest.Slug,
                PublishedDate = blogPostRequest.PublishedDate,
                Author = blogPostRequest.Author,
                Visible = blogPostRequest.Visible
            };

            List<Tag> selectedTags = new();

            foreach (var selectedTagId in blogPostRequest.SelectedTags)
            {
                var existingTag = await tagRepository.GetAsync(Guid.Parse(selectedTagId));

                if(existingTag != null) 
                {
                    selectedTags.Add(existingTag);
                }
            }

            blogPost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPost);   

            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            var posts = await blogPostRepository.GetAllAsync();

            return View(posts);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            var tags = await tagRepository.GetAllAsync();

            if(blogPost != null) 
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Slug = blogPost.Slug,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tags.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPost)
        {
            var selectedTags = new List<Tag>();

            var blogPost = new BlogPost()
            {
                Id = editBlogPost.Id,
                Heading = editBlogPost.Heading,
                Title = editBlogPost.Title,
                Content = editBlogPost.Content,
                Author = editBlogPost.Author,
                ShortDescription = editBlogPost.ShortDescription,
                FeaturedImageUrl = editBlogPost.FeaturedImageUrl,
                PublishedDate = editBlogPost.PublishedDate,
                Slug = editBlogPost.Slug,
                Visible = editBlogPost.Visible,
            };

            foreach(var tag in editBlogPost.SelectedTags)
            {
                if(Guid.TryParse(tag, out Guid result))
                {
                    var foundTag = await tagRepository.GetAsync(result);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }

            }

            blogPost.Tags = selectedTags; 

            var updatedBlog = await blogPostRepository.UpdateAsync(blogPost);

            if (updatedBlog != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPost)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPost.Id);

            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editBlogPost.Id });
        }
    }
}
