using coreFormValidation.Models;
using Microsoft.AspNetCore.Mvc;


namespace coreFormValidation.Controllers
{
    public class AccountController : Controller
    {
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
    }
}
