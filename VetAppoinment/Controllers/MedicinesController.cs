using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetAppoinment.Models.DbContext;
using VetAppoinment.Models.Dtos.Medicine;
using VetAppoinment.Models.Entities;

namespace VetAppoinment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetMedicines()
        {
            return await _context.Medicines.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicine>> GetMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            return medicine;
        }

        [HttpPost]
        public async Task<ActionResult<Medicine>> PostMedicine(MedicineCreateDTO medicineDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicine = new Medicine
            {
                Name = medicineDTO.Name,
                Description = medicineDTO.Description,
                Email = medicineDTO.Email,
                Price = medicineDTO.Price,
                Image = medicineDTO.Image,
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, medicine);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicine(int id, MedicineUpdateDTO medicineDTO)
        {
            if (id != medicineDTO.Id)
            {
                return BadRequest();
            }

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            medicine.Name = medicineDTO.Name;
            medicine.Description = medicineDTO.Description;
            medicine.Email = medicineDTO.Email;
            medicine.Price = medicineDTO.Price;
            medicine.Image = medicineDTO.Image;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }

    }
}
