using coreFormValidation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace coreFormValidation.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger) 
        { 
            _logger = logger; 
        }

        public IActionResult WeeklyTypedLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPost(string username, string password)
        {
            ViewBag.Username = username;
            ViewBag.Password = password;
            return View();
        }

        public IActionResult StronglyTypedLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginSuccess(LoginViewModel login)
        {          
            ViewBag.Username = login.Username;
            ViewBag.Password = login.Password;
            return View("LoginPost");          
        }

        public IActionResult UserDetail()
        {
            var user = new LoginViewModel()
            {
                Username = "JohnDoe",
                Password = "Secure"
            };
            return View(user);
        }

        public IActionResult UsersList()
        {
            var users = new List<LoginViewModel>
            {
                new LoginViewModel { Username = "Alice", Password = "Password1" },
                new LoginViewModel { Username = "Bob", Password = "Password2" },
                new LoginViewModel { Username = "Charlie", Password = "Password3" }
            };
            return View(users);
        }

        public IActionResult GetAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostAccount(Account account)
        {

            _logger.LogInformation($"Username: {account.Username}");
            _logger.LogInformation($"Password: {account.Password}");
            _logger.LogInformation($"Age: {account.Age}");
            _logger.LogInformation($"Email: {account.Email}");
            _logger.LogInformation($"Website: {account.Website}");
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Account created successfully!";
                return View("Success");
            }
            else
            {
                _logger.LogInformation("Validation Errors:");
                ViewBag.Message = "There are validation errors.";
                return RedirectToAction("GetAccount");
            }
            
        }
    }
}
