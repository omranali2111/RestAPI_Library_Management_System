using System.ComponentModel.DataAnnotations;

namespace RestAPI_Library_Management_System.models
{
    public class Patron
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(255)]
        public string ContactNumber { get; set; }

        [Range(0, 150)]  
        public int Age { get; set; }


    }
}
