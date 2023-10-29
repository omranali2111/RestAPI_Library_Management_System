using System.ComponentModel.DataAnnotations;

namespace RestAPI_Library_Management_System.models
{
    public class SignIn
    {

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
