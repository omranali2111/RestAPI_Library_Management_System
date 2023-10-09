using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestAPI_Library_Management_System.models
{
    public class User
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
