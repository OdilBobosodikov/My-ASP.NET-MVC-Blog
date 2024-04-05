using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.web.Models.Domain;
using MyBlog.web.Models.ViewModels;
using MyBlog.web.Repositories.Interfaces;
using System.Net;

namespace MyBlog.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository blogPostLikeRepository;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostLikeRepository = blogPostLikeRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new BlogPostLike()
            {
                BlogPostId = addLikeRequest.BlogPostId,
                UserId = addLikeRequest.UserId
            };

            await blogPostLikeRepository.AddLikeForBlog(model);

            return Ok();
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> RemoveLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var like = await blogPostLikeRepository.GetLike(addLikeRequest.BlogPostId);

            if (like != null)
            {
                await blogPostLikeRepository.DeleteLike(like);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
        {
            var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPostId);

            return Ok(totalLikes);
        }
    }
}
