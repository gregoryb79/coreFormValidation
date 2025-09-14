//using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace coreFormValidation.Models
{
    public class Account
    {
        public string _id { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string eMail { get; set; } = string.Empty;
        [Required]        
        public string hashedPassword { get; set; } = string.Empty;
        [Required]
        public string[] ToDoItemsIDs { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
    }
}
