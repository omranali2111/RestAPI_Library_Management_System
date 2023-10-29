using System.ComponentModel.DataAnnotations;

namespace RestAPI_Library_Management_System.models
{
    public class SignUp
    {
        [Required]
        public string Name { get; set; }

        [MaxLength(255)]
        public string ContactNumber { get; set; }
        [Range(0, 150)]
        public int Age { get; set; }
        [Required]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
