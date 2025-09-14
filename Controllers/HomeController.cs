using coreFormValidation.Models;
using coreFormValidation.Services;
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

        public IActionResult Index()
        {
            var sortedList = _mongoService.GetAll()
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

            return RedirectToAction("Index");
        }

        [HttpPost]
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
        public IActionResult RemoveItem(string id)
        {
            Console.WriteLine($"Removing item {id}");
            //var item = _mongoService.GetById(id);
            //if (item != null)
            //{
                _mongoService.Delete(id);
            //}
            
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
