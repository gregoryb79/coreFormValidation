using System.ComponentModel.DataAnnotations;

namespace coreFormValidation.Models
{
    public class ToDoItem
    {
        public string _id { get; set; } = string.Empty;
        [Required(ErrorMessage = "Task description is required.")]
        [StringLength(200, ErrorMessage = "Task description cannot exceed 200 characters.")]
        [MinLength(3, ErrorMessage = "Task description must be at least 3 characters long.")]
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
    }
}
