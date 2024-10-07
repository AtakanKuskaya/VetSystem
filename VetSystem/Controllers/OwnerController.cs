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
    //[Authorize]
    public class OwnersController : ControllerBase
    {
        private readonly DataContext _context;

        public OwnersController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateOwner([FromBody] OwnerDto ownerDto)
        {
            var owner = new Owner { Name = ownerDto.Name, Phone = ownerDto.Phone };
            _context.Owners.Add(owner);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetOwners), new { id = owner.OwnerId }, owner);
        
        }

        [HttpGet]
        public IActionResult GetOwners()
        {
            var owners = _context.Owners.Include(o => o.Animals).ToList();
            return Ok(owners);
        }

        [HttpPost("{ownerId}/animals")]
        public IActionResult AddAnimal(int ownerId, [FromBody] AnimalDto animalDto)
        {
            var owner = _context.Owners.Include(o => o.Animals).FirstOrDefault(o => o.OwnerId == ownerId);
            if (owner == null) return NotFound();

            var animal = new Animal { Name = animalDto.Name, Species = animalDto.Species, Gender = animalDto.Gender, Age = animalDto.Age };
            owner.Animals.Add(animal);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetOwners), new { id = owner.OwnerId }, animal);
        }
    }
}
