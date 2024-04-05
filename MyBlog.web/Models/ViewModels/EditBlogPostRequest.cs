using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyBlog.web.Models.ViewModels
{
    public class EditBlogPostRequest
    {
        public Guid Id { get; set; }

        public string Heading { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string Slug { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        public IEnumerable<SelectListItem> Tags { get; set; }
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
