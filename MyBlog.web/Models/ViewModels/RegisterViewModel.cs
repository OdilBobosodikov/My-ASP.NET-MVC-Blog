using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.web.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 5 characters")]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        //For some reason Remote validation does not work :(
        //[Remote("IsEmailUnique", "Account", ErrorMessage = "Email is already in use.")]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).+$", ErrorMessage = "Password must contain upper case letter, digit and at least one special character")]
        public string Password { get; set; }


        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).+$", ErrorMessage = "Password must contain upper case letter, digit and at least one special character")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
