using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NopBookStore.Data;
using NopBookStore.Models;
using NopBookStore.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace NopBookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly ModernBookShopDbContext modernBookShopDbContext;
        public UserController(ModernBookShopDbContext context)
        {
            modernBookShopDbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                // Authenticate the user (you need to replace this with your actual authentication logic)
                var user = await AuthenticateUser(userLoginViewModel.UserEmail, userLoginViewModel.UserPassword);

                if (user != null)
                {
                    // Create claims for the authenticated user
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        
                // Add more claims as needed
            };

                    var claimsIdentity = new System.Security.Claims.ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Create authentication properties
                    var authProperties = new AuthenticationProperties
                    {
                        // Set additional properties as needed
                    };

                    // Sign in the user
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new System.Security.Claims.ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Redirect to the Index action of the Book controller upon successful login
                    return RedirectToAction("Index", "Book");
                }
            }

            // If authentication fails, return the login view with errors
            return View(userLoginViewModel);
        }

        private async Task<User> AuthenticateUser(string userEmail, string password)
        {
            // Replace this with your actual authentication logic using your data store
            var user = await modernBookShopDbContext.Users.FirstOrDefaultAsync(u =>
                u.UserEmail == userEmail && u.UserPassword == password);

            return user;
        }
        public IActionResult DashBoard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.mySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }

            return View();
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpViewModel userSignUpViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = userSignUpViewModel.UserName,
                    UserEmail = userSignUpViewModel.UserEmail,
                    UserPassword = userSignUpViewModel.UserPassword
                };
                modernBookShopDbContext.Add(User);
                await modernBookShopDbContext.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(userSignUpViewModel);


        }

        //public async Task<IActionResult> DeleteBook(int bookid)
        //{
        // get user  
        // get user roles from user-role table 
        // get permission from role-permission table 

        // if permission not null 
        // delete

        // otherwise dont delete
        //}
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the home page or any other desired page
            return RedirectToAction("Index", "Home");
        }
    }
}
