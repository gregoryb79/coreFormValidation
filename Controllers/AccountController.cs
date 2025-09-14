using coreFormValidation.Models;
using coreFormValidation.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;


namespace coreFormValidation.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly MongoDbService _mongoService;

        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();

        public AccountController(MongoDbService mongoService, ILogger<AccountController> logger) 
        {
            _mongoService = mongoService;
            _logger = logger; 
        }
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult LoginPost(LoginViewModel loginData)
        {
            Console.WriteLine($"e-mail: {loginData.eMail}, password: {loginData.Password}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Validation error!");
                ViewBag.ErrorMessage = "Wrong Input";                
                return View("Login",loginData);
            }

            return View("Login",loginData);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterPost(RegisterViewModel registerData)
        {            
            Console.WriteLine($"e-mail: {registerData.eMail}, password: {registerData.Password}, confirm password: {registerData.ConfirmPassword}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Validation error!");
                ViewBag.ErrorMessage = "Wrong Input";                
                return View("Register",registerData);
            }
            var existingUser = _mongoService.GetAllAccounts().FirstOrDefault(user => user.eMail == registerData.eMail);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Account error";
                ModelState.AddModelError("eMail", "E-mail is already registered");
                return View("Register",registerData);
            }
            var newUser = new Account();
            newUser.eMail = registerData.eMail;
            newUser.hashedPassword = _hasher.HashPassword(null,registerData.Password);
            newUser._id = Guid.NewGuid().ToString();
            _mongoService.AddAccount(newUser);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, newUser.eMail),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }    

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
