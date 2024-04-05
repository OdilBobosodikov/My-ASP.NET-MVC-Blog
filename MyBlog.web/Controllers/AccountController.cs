using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.web.Models.ViewModels;
using MyBlog.web.Repositories.Interfaces;

namespace MyBlog.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IUserRepository userRepository;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerView) 
        {
            await ValidateEmailField(registerView.Email);
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = registerView.Username,
                    Email = registerView.Email
                };

                var identityResult = await userManager.CreateAsync(user, registerView.Password);

                if (identityResult.Succeeded)
                {
                    var roleIdentityResult = await userManager.AddToRoleAsync(user, "User");

                    if (roleIdentityResult.Succeeded)
                    {
                        var signInResult = await signInManager.PasswordSignInAsync(registerView.Username, registerView.Password, false, false);
                        if (signInResult.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginView)
        {
            await ValidateUsernameField(loginView.Username);
            await ValidatePasswordField(loginView.Username, loginView.Password);
            if (ModelState.IsValid)
            {
                var signInResult = await signInManager.PasswordSignInAsync(loginView.Username, loginView.Password, false, false);

                if (signInResult != null && signInResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(loginView.ReturnUrl))
                    {
                        return Redirect(loginView.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task ValidateEmailField(string email)
        {
            if (email == null)
            {
                return;
            }

            var user = await userManager.FindByEmailAsync(email);

            if (user != null) 
            {
                ModelState.AddModelError("Email", "Email is already taken");
            }
        }
        private async Task ValidateUsernameField(string username)
        {
            if (username == null)
            {
                return;
            }

            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                ModelState.AddModelError("Username", "There is no such user");
            }
        }

        private async Task ValidatePasswordField(string username, string password)
        {
            if (password == null || username == null)
            {
                return;
            }

            var user = await userManager.FindByNameAsync(username);

            if (user != null)
            {
                var identityResult = await userManager.CheckPasswordAsync(user, password);

                if (!identityResult)
                {
                    ModelState.AddModelError("Password", "Password does not match username");
                }
            }
        }
    }
}
