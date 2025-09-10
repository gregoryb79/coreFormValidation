//using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace coreFormValidation.Models
{
    public class Account
    {
        [Required]
        [MinLength(5)] 
        [MaxLength(20)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Range(18,99)]        
        public int Age { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Url]
        public string Website { get; set; } = string.Empty;
    }
}
