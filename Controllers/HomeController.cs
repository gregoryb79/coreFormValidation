using coreFormValidation.Models;
using coreFormValidation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace coreFormValidation.Controllers
{
    public class HomeController : Controller
    {
        private static List<ToDoItem> _toDoList = new List<ToDoItem>();

        private readonly ILogger<HomeController> _logger;
        private readonly MongoDbService _mongoService;

        public HomeController(MongoDbService mongoService, ILogger<HomeController> logger)
        {
            _mongoService = mongoService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userEmail = User.Identity?.Name ?? "unknown";
            Console.WriteLine($"User {userEmail} accessed the To-Do list.");
            var taskIds = _mongoService.GetAllAccounts()
                .FirstOrDefault(user => user.eMail == userEmail)?.ToDoItemsIDs;
            if (taskIds == null || taskIds.Length == 0)
            {
                Console.WriteLine("No tasks found for this user.");
                var newModel = new ToDoListViewModel();
                return View(newModel);
            }

            var sortedList = _mongoService.GetAll()
                .Where(item => taskIds != null && taskIds.Contains(item._id))
                .OrderBy(item => item.IsCompleted) // false first, then true
                .ToList();
            
            var model = new ToDoListViewModel
            {
                NewItem = new ToDoItem(),
                Items = sortedList
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddItem(ToDoListViewModel model)
        {
            Console.WriteLine($"Received Task: {model.NewItem.Task}, IsCompleted: {model.NewItem.IsCompleted} createdAt: {model.NewItem.CreatedAt}");
            //var newModel = new ToDoListViewModel
            //{
            //    NewItem = new ToDoItem(),
            //    Items = _toDoList
            //};
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Validation error!");
                ViewBag.ErrorMessage = "Wrong Input";
                model.Items = _toDoList;
                return View("Index");
            }

            model.NewItem._id = Guid.NewGuid().ToString();
            _mongoService.Add(model.NewItem);            

            var account = _mongoService.GetAllAccounts()
                .FirstOrDefault(user => user.eMail == User.Identity?.Name);
            if (account == null)
            { 
                Console.WriteLine("Account not found for the current user.");
                return RedirectToAction("Index");
            }
            account.ToDoItemsIDs = (account.ToDoItemsIDs ?? Array.Empty<string>()).Append(model.NewItem._id).ToArray();
            _mongoService.UpdateAccount(account._id, account);
            

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult ToggleCompleted(string id, bool isCompleted)
        {   
            Console.WriteLine($"Toggling item {id} to {(isCompleted ? "completed" : "not completed")}");
            var item = _mongoService.GetById(id);
            if (item != null)
            {
                item.IsCompleted = isCompleted;
                item.LastUpdatedAt = DateTime.Now;
                _mongoService.Update(id,item);
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult RemoveItem(string id)
        {
            Console.WriteLine($"Removing item {id}");
            //var item = _mongoService.GetById(id);
            //if (item != null)
            //{
                _mongoService.Delete(id);
            //}
            var account = _mongoService.GetAllAccounts()
                .FirstOrDefault(user => user.eMail == User.Identity?.Name);
            if (account != null && account.ToDoItemsIDs != null && account.ToDoItemsIDs.Contains(id))
            {
                account.ToDoItemsIDs = account.ToDoItemsIDs.Where(itemId => itemId != id).ToArray();
                _mongoService.UpdateAccount(account._id, account);
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
