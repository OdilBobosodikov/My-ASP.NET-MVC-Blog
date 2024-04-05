namespace MyBlog.web.Repositories.Interfaces
{
    public interface IImageRepository 
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
