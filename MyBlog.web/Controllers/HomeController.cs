using Microsoft.AspNetCore.Mvc;
using MyBlog.web.Models;
using MyBlog.web.Models.ViewModels;
using MyBlog.web.Repositories.Interfaces;
using System.Diagnostics;

namespace MyBlog.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index(string? tagName)
        {
            var posts = await blogPostRepository.GetAllAsync();
            ViewBag.SelectedTagName = tagName;

            if (tagName != null)
            {
                posts = await blogPostRepository.GetAllByTagName(tagName);
            }

            var tags = await tagRepository.GetAllAsync();

            HomeViewModel model = new() 
            {
                BlogPosts = posts,
                Tags = tags,
            }; 

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
