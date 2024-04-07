using Microsoft.AspNetCore.Mvc;
using MyBlog.web.Models;
using MyBlog.web.Models.ViewModels;
using MyBlog.web.Repositories.Interfaces;
using System.Diagnostics;
using System.Globalization;

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

        [HttpGet]
        public async Task<IActionResult> Index(string? searchQuery, string? tagName, int pageNumber = 1, int pageSize = 5)
        {    
            var totalRecords = await blogPostRepository.CountAsync(tagName);
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber--;
            }

            if (pageNumber < 1)
            {
                pageNumber++;
            }

            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.SelectedTagName = tagName;

            var posts = await blogPostRepository.GetAllAsync(searchQuery: searchQuery,
                                                 pageNumber: pageNumber,
                                                 pageSize: pageSize,
                                                 tagName: tagName);
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
