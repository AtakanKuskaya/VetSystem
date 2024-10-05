using System.ComponentModel.DataAnnotations;

namespace VetSystem.Entities
{
    public class Owner
    {
        [Key]
        public int OwnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        // Navigation Property
        public ICollection<Animal> Animals { get; set; }
    }
}
