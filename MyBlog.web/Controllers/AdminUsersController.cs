using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.web.Models.ViewModels;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserRepository userRepository,
            UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await userRepository.GetAll();

            var userView = users
                .Select(x => new UserViewModel() 
                { 
                    Id = Guid.Parse(x.Id),
                    Email = x.Email,
                    Username = x.UserName 
                }).ToList();
            
            var userViewList = new ListOfUsersViewModel { Users = userView};

            return View(userViewList);
        }

        [HttpPost]
        public async Task<IActionResult> List(ListOfUsersViewModel request)
        {
            if (request.Password == null)
            {
                return RedirectToAction("List", "AdminUsers");
            }

            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult != null && identityResult.Succeeded)
            {
                var roles = new List<string> { "User" };

                if (request.AdminRoleEnabled)
                {
                    roles.Add("Admin");
                }

                await userManager.AddToRolesAsync(identityUser, roles);

                //identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                //if (identityResult != null && identityResult.Succeeded)
                //{
                //    succeeded notification
                //    return RedirectToAction("List", "AdminUsers");
                //}
            }
            //error notofication
            return RedirectToAction("List", "AdminUsers");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult != null && identityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }

            return View();
        }

    }
}
