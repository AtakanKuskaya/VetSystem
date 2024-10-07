using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetSystem.Data;
using VetSystem.Dto;
using VetSystem.Entities;

namespace VetSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class AnimalsController : ControllerBase
    {
        private readonly DataContext _context;

        public AnimalsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("{animalId}/records")]
        public IActionResult AddRecord(int animalId, [FromBody] RecordDto recordDto)
        {
            var animal = _context.Animals.FirstOrDefault(a => a.AnimalId == animalId);
            if (animal == null) return NotFound((new { message = "Owner not found." }));

            var record = new Record { AnimalId = animalId, Diagnosis = recordDto.Diagnosis, Treatment = recordDto.Treatment, Date = DateTime.Now };
            _context.Records.Add(record);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetRecords), new {animalId }, record);
        }

        [HttpGet("{animalId}/records")]
        public IActionResult GetRecords(int animalId)
        {
            var record = _context.Records.Include(a => a.Animal).FirstOrDefault(a => a.AnimalId == animalId);
            if (record == null)
            {
                return NotFound(new { message = "Animal not found." });
            }
            if(record == null)
            {
                return NotFound(new { message = "Record not found for this animal." });
            }
            return Ok(record);
        }
    }
}