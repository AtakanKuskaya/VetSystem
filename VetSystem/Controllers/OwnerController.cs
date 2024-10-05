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
    public class OwnersController : ControllerBase
    {
        private readonly DataContext _context;

        public OwnersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/owners
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Owner>>> GetOwners()
        {
            return await _context.Owners.Include(o => o.Animals).ToListAsync();
        }

        // POST: api/owners
        [HttpPost]
        public async Task<ActionResult<Owner>> PostOwner(Owner owner)
        {
            if (owner == null)
                return NotFound(new { message = "Owner not found." });

            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOwners), new { id = owner.OwnerId }, owner);
        }
        
        // POST: api/owners/{ownerId}/animals
        [HttpPost("{ownerId}/animals")]
         public async Task<ActionResult<Animal>> PostAnimal(int ownerId, Animal animal)
         {
             var owner = await _context.Owners.FindAsync(ownerId);
             if (owner == null)
             {
                 return NotFound(new { message = "Owner not found." });
             }

             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             animal.OwnerId = ownerId;
             _context.Animals.Add(animal);
             await _context.SaveChangesAsync();

             return CreatedAtAction(nameof(AnimalsController.GetRecords), "Animals", new { animalId = animal.AnimalId }, animal);
         }
       
        /*
        //To Get All Owners
        [HttpGet("Get All Owners")]
        public async Task<ActionResult<List<Owner>>> GetAllOwners()
        {
            var owners = await _context.Owners.ToListAsync();
            return Ok(owners);
        }

        //To Get Spesific Owner
        [HttpGet("Owner/{ownerId}")]
        public async Task<ActionResult<Owner>> GetOwner(int ownerid)
        {
            var owner = await _context.Owners.FindAsync(ownerid);
            if (owner == null)
                return NotFound(new { message = "Owner not found." });

            return Ok(owner);
        }
        //To Add Owner
        [HttpPost]
        public async Task<ActionResult<List<Owner>>> AddOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();


            return Ok(await _context.Owners.ToListAsync());
        }
        //To Update Owner
        [HttpPut("Update Owner/{ownerId}")]
        public async Task<ActionResult<List<Owner>>> UpdateOwner(Owner updatedowner)
        {
            var dbowner = await _context.Owners.FindAsync(updatedowner.OwnerId);
            if (dbowner == null)
                return NotFound(new { message = "Owner not found." });

            dbowner.Name = updatedowner.Name;
            dbowner.Phone = updatedowner.Phone;
            await _context.SaveChangesAsync();
            return Ok(await _context.Owners.ToListAsync());
        }
        //To Delete Owner
        [HttpDelete("Delete Owner/{ownerId}")]
        public async Task<ActionResult<List<Owner>>> DeleteOwner(int id)
        {
            var dbOwner = await _context.Owners.FindAsync(id);
            if (dbOwner == null)
                return NotFound(new { message = "Owner not found." });

            _context.Owners.Remove(dbOwner);
            await _context.SaveChangesAsync();
            return Ok(await _context.Owners.ToListAsync());
        }*/
    }
}