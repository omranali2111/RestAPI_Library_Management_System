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

        [MaxLength(255)]
        public string ContactNumber { get; set; }

    }
}
