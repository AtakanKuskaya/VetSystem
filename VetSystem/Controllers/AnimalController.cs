using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetSystem.Data;
using VetSystem.Entities;

namespace VetSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnimalsController : ControllerBase
    {
        private readonly DataContext _context;

        public AnimalsController(DataContext context)
        {
            _context = context;
        }

       // POST: api/animals/{animalId}/records
        [HttpPost("{animalId}/records")]
        public async Task<ActionResult<Record>> PostRecord(int animalId, Record record)
        {
            var animal = await _context.Animals.FindAsync(animalId);
            if (animal == null)
            {
                return NotFound(new { message = "Animal not found." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            record.AnimalId = animalId;
            _context.Records.Add(record);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecords), new { animalId = record.AnimalId }, record);
        }

        // GET: api/animals/{animalId}/records
        [HttpGet("{animalId}/records")]
        public async Task<ActionResult<IEnumerable<Record>>> GetRecords(int animalId)
        {
            var animal = await _context.Animals.FindAsync(animalId);
            if (animal == null)
                 return NotFound(new { message = "Animal not found." });
            

            var records = await _context.Records
                .Where(r => r.AnimalId == animalId)
                .ToListAsync();

            return Ok(records);
        }

       /* //To Get All Animals
        [HttpGet("Get All Animals")]
        public async Task<ActionResult<List<Animal>>> GetAllAnimals()
        {
            var animals = await _context.Animals.ToListAsync();
            return Ok(animals);
        }

        //To Get Spesific Animal
        [HttpGet("animal/{animalId}")]
        public async Task<ActionResult<Animal>> GetAnimal(int animalId)
        {
            var animal = await _context.Animals.FindAsync(animalId);
            if (animal == null)
                return NotFound(new { message = "Animal not found." });
            
            return Ok(animal);
        }
        //To Add Animal
        [HttpPost]
        public async Task<ActionResult<List<Animal>>> AddAnimal(Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();


            return Ok(await _context.Animals.ToListAsync());
        }
        //To Update Animal
        [HttpPut]
        public async Task<ActionResult<List<Animal>>> UpdateAnimal(Animal updatedanimal)
        {
            var dbAnimal = await _context.Animals.FindAsync(updatedanimal.AnimalId);
            if (dbAnimal == null)
                return NotFound(new { message = "Animal not found." });

            dbAnimal.Name= updatedanimal.Name;
            dbAnimal.Age= updatedanimal.Age;
            await _context.SaveChangesAsync();
            return Ok(await _context.Animals.ToListAsync());
        }
        //To Delete Animal
        [HttpDelete]
        public async Task<ActionResult<List<Animal>>> DeleteAnimal(int id)
        {
            var dbAnimal = await _context.Animals.FindAsync(id);
            if (dbAnimal == null)
                return NotFound(new { message = "Animal not found." });

            _context.Animals.Remove(dbAnimal);
            await _context.SaveChangesAsync();
            return Ok(await _context.Animals.ToListAsync());
        }*/
    }
}