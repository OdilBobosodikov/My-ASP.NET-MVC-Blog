using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.web.Models.Domain;
using MyBlog.web.Models.ViewModels;
using MyBlog.web.Repositories.Classes;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository postLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,
                               IBlogPostLikeRepository postLikeRepository,
                               SignInManager<IdentityUser> signInManager,
                               UserManager<IdentityUser> userManager,
                               IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.postLikeRepository = postLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            var liked = false;
            var blog = await blogPostRepository.GetBySlugAsync(slug);
            var blogPostDetails = new BlogDetailsViewModel();

            if (blog != null)
            {
                if (signInManager.IsSignedIn(User))
                {
                    var likes = await postLikeRepository.GetLikesForBlog(blog.Id);
                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFromUser = likes.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }

                var blogComments = blogPostCommentRepository.GetAllByIdAsync(blog.Id);

                var blogCommentsViewModel = new List<BlogComment>();
                
                foreach (var comment in await blogComments) 
                {
                    blogCommentsViewModel.Add(new BlogComment
                    {
                        Description = comment.Description,
                        DateAdded = comment.DateAdded,
                        Username = (await userManager.FindByIdAsync(comment.UserId.ToString())).UserName
                    }); 
                }

                blogPostDetails = new BlogDetailsViewModel()
                {
                    Id = blog.Id,
                    Content = blog.Content,
                    Title = blog.Title,
                    PublishedDate = blog.PublishedDate,
                    Author = blog.Author,
                    Slug = blog.Slug,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    Heading = blog.Heading,
                    ShortDescription = blog.ShortDescription,
                    Visible = blog.Visible,
                    Tags = blog.Tags,
                    TotalLikes = await postLikeRepository.GetTotalLikes(blog.Id),
                    Liked = liked,
                    Comments = blogCommentsViewModel
                };
            }

            return View(blogPostDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var model = new BlogPostComment()
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await blogPostCommentRepository.AddAsync(model);
                return RedirectToAction("Index", "Blogs", new {slug = blogDetailsViewModel.Slug });
            }
            return View();
        }
    }
}
