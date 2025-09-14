using System.ComponentModel.DataAnnotations;

namespace coreFormValidation.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Pease enter valid e-mail")]
        [EmailAddress(ErrorMessage = "Invalid e-mail address")]
        public string eMail { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        [Length(6,100, ErrorMessage = "Password must be at least 6 characters long")]

        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
