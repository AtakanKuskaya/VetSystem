using System.ComponentModel.DataAnnotations;

namespace VetSystem.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } 
    }
}
