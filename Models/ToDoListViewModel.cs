namespace coreFormValidation.Models
{
    public class ToDoListViewModel
    {
        public ToDoItem NewItem { get; set; } = new ToDoItem();
        public List<ToDoItem> Items { get; set; } = new List<ToDoItem>();
    }
}
