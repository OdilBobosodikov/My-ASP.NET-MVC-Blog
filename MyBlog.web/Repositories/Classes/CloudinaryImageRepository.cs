using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Repositories.Classes
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;

        public CloudinaryImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;

            account = new Account(configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"], 
                configuration.GetSection("Cloudinary")["ApiSecret"]);



        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            Cloudinary cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);


            if(uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK) 
            {
                return uploadResult.SecureUri.ToString();
            }

            return null;
        }
    }
}
