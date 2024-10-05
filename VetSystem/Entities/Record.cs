using System.ComponentModel.DataAnnotations;

namespace VetSystem.Entities
{
    public class Record
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        public int AnimalId { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public string Treatment { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public Animal Animal { get; set; }
    }
}
