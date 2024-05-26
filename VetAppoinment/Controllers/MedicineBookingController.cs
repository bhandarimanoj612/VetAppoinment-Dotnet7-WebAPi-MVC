using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VetAppoinment.Models.DbContext;
using VetAppoinment.Models.Dtos.Medicine;
using VetAppoinment.Models.Entities;

namespace VetAppoinment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineBookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicineBookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Book")]
        public async Task<IActionResult> BookMedicine(MedicineBookingDTO bookingDTO)
        {
            var medicine = await _context.Medicines.FirstOrDefaultAsync(m => m.Id == bookingDTO.MedicineId);
            if (medicine == null)
            {
                return NotFound($"Medicine with ID '{bookingDTO.MedicineId}' not found.");
            }

            var booking = new MedicineBooking
            {
                MedicineId = bookingDTO.MedicineId,
                MedicineName = medicine.Name,
                UserName = bookingDTO.UserName,
                Price = medicine.Price,
                Image = medicine.Image,
                Email = medicine.Email,
            };

            _context.MedicineBookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok("Medicine booked successfully.");
        }

        [HttpDelete("Cancel/{id}")]
        public async Task<IActionResult> CancelMedicine(int id)
        {
            var booking = await _context.MedicineBookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound("Medicine booking not found.");
            }

            _context.MedicineBookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok("Medicine booking canceled successfully.");
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<MedicineBooking>>> GetAllBookings()
        {
            var bookings = await _context.MedicineBookings.ToListAsync();
            return bookings;
        }

        [HttpGet("ByUser/{username}")]
        public async Task<ActionResult<IEnumerable<MedicineBooking>>> GetBookingsByUser(string username)
        {
            var bookings = await _context.MedicineBookings.Where(b => b.UserName == username).ToListAsync();
            return bookings;
        }

        [HttpGet("Invoice/{id}")]
        public async Task<IActionResult> GenerateInvoice(int id)
        {
            var booking = await _context.MedicineBookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound("Medicine booking not found.");
            }

            // Create PDF document
            var document = new PdfDocument();
            var page = document.AddPage();
            var graphics = XGraphics.FromPdfPage(page);
            var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
            var fontHeading = new XFont("Arial", 14, XFontStyle.Bold);
            var fontNormal = new XFont("Arial", 12, XFontStyle.Regular);

            // Define invoice title
            var title = "Invoice for Medicine Booking";

            // Draw title on PDF page
            graphics.DrawString(title, fontTitle, XBrushes.Black, new XRect(30, 30, page.Width.Point, 30), XStringFormats.TopLeft);

            // Draw invoice number
            graphics.DrawString($"Invoice Number: {id}", fontNormal, XBrushes.Black, new XRect(30, 70, page.Width.Point, 30), XStringFormats.TopLeft);

            // Draw booking details heading
            graphics.DrawString("Booking Details", fontHeading, XBrushes.Black, new XRect(30, 100, page.Width.Point, 30), XStringFormats.TopLeft);

            // Define booking details content
            var bookingDetails = $"Name: {booking.MedicineName}{Environment.NewLine}{Environment.NewLine}" +
                                 $"UserName: {booking.UserName}{Environment.NewLine}{Environment.NewLine}" +
                                 $"Book Date: {booking.BookingDate:MM/dd/yyyy HH:mm}{Environment.NewLine}{Environment.NewLine}" +
                                 $"Price: {booking.Price:C}{Environment.NewLine}{Environment.NewLine}";

            // Draw booking details content on PDF page
            graphics.DrawString(bookingDetails, fontNormal, XBrushes.Black, new XRect(30, 130, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

            // Draw contact information
            graphics.DrawString("Contact Information", fontHeading, XBrushes.Black, new XRect(30, 250, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("www.vetAppointment.com", fontNormal, XBrushes.Black, new XRect(30, 280, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("7 jhapa", fontNormal, XBrushes.Black, new XRect(30, 300, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("nepal, province 1, 23322", fontNormal, XBrushes.Black, new XRect(30, 320, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("Phone: 98132981321", fontNormal, XBrushes.Black, new XRect(30, 340, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("Email: vetAppointment@example.com", fontNormal, XBrushes.Black, new XRect(30, 360, page.Width.Point, 30), XStringFormats.TopLeft);

            // Save PDF to memory stream
            using (var memoryStream = new MemoryStream())
            {
                document.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Return PDF as a file attachment
                return File(memoryStream.ToArray(), "application/pdf", $"Invoice_{id}.pdf");
            }
        }
    }
}
