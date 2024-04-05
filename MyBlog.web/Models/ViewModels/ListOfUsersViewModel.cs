namespace MyBlog.web.Models.ViewModels
{
    public class ListOfUsersViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool AdminRoleEnabled { get; set; }
    }
}
