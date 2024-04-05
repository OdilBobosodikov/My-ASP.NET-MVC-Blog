using System.ComponentModel.DataAnnotations;

namespace MyBlog.web.Models.ViewModels
{
    public class LoginViewModel
    {
        public string? ReturnUrl { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at aleast 6 characters")]
        public string Password { get; set; }
    }
}
