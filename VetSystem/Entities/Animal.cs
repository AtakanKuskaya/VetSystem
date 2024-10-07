using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VetSystem.Entities
{
    public class Animal
    {
        
        [Key]
        public int AnimalId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Species { get; set; }

        [Required]
        public required string Gender { get; set; }

        [Required]
        public required int Age { get; set; }

        // Foreign Key
        public int OwnerId { get; set; }

        // Navigation Properties
         //public virtual Owner  Owner { get; set; }
        //public ICollection<Record>? Records { get; set; }

    }
}
