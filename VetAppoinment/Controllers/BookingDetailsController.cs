using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetAppoinment.Models.DbContext;
using VetAppoinment.Models.Entities;

namespace VetAppoinment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("MedicineByUser")]
        public async Task<ActionResult<IEnumerable<MedicineBooking>>> GetMedicineBookingsByEmail([FromQuery] string email)
        {
            var medicineBookings = await _context.MedicineBookings
                .Where(mb => mb.Email == email)
                .ToListAsync();

            return medicineBookings;
        }

        [HttpGet("AppointmentByUser")]
        public async Task<ActionResult<IEnumerable<AppointmentBooking>>> GetAppointmentBookingsByEmail([FromQuery] string email)
        {
            var appointmentBookings = await _context.AppointmentBookings
                .Where(ab => ab.Email == email)
                .ToListAsync();

            return appointmentBookings;
        }


        [HttpGet("ByUser")]
        public async Task<ActionResult<IEnumerable<object>>> GetBookingsByEmail([FromQuery] string email)
        {
            var medicineBookings = await _context.MedicineBookings
                .Where(mb => mb.Email == email)
                .Select(mb => new
                {
                    Id = mb.Id,
                    Type = "Medicine",
                    Name = mb.MedicineName,
                    UserName = mb.UserName,
                    BookingDate = mb.BookingDate,
                    Price = mb.Price,
                    Image = mb.Image
                })
                .ToListAsync();

            var appointmentBookings = await _context.AppointmentBookings
                .Where(ab => ab.Email == email)
                .Select(ab => new
                {
                    Id = ab.Id,
                    Type = "Appointment",
                    Name = ab.VetName,
                    UserName = ab.UserName,
                    BookingDate = ab.BookingDate,
                    Price = ab.Price,
                    Image = ab.Image
                })
                .ToListAsync();

            var combinedBookings = medicineBookings.Concat(appointmentBookings).OrderBy(b => b.BookingDate);

            return combinedBookings.ToList();
        }

         }
}

