using System.ComponentModel.DataAnnotations;

namespace OrderEase.Models.Authentication.SignUp
{
    public class RegisterUser
    {
        [Required(ErrorMessage ="Username is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }    
    }
}
