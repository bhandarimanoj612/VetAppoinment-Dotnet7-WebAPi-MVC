using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Linq;
using System.Threading.Tasks;
using VetAppoinment.Models.DbContext;
using VetAppoinment.Models.Dtos.Appoinment;
using VetAppoinment.Models.Entities;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;


namespace VetAppoinment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentBookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentBookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Book")]
        
        public async Task<IActionResult> BookAppointment(AppointmentBookingDTO bookingDTO)
        {
            var appointment = await _context.Appointments.FindAsync(bookingDTO.AppointmentId);
            if (appointment == null)
            {
                return NotFound($"Appointment with ID '{bookingDTO.AppointmentId}' not found.");
            }

            var booking = new AppointmentBooking
            {
                AppointmentId = bookingDTO.AppointmentId,
                BookStatus = bookingDTO.BookStatus,
                UserName = bookingDTO.UserName,
                Price = bookingDTO.Price,
                Image = bookingDTO.Image,
                Email = bookingDTO.Email,
                VetName=bookingDTO.VetName
            };

            _context.AppointmentBookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok("Appointment booked successfully.");
        }

        [HttpDelete("Cancel/{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var booking = await _context.AppointmentBookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            _context.AppointmentBookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok("Appointment canceled successfully.");
        }

        // GET: api/AppointmentBooking/All
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<AppointmentBooking>>> GetAllBookings()
        {
            var bookings = await _context.AppointmentBookings.ToListAsync();
            return bookings;
        }

        // GET: api/AppointmentBooking/ByUser/{username}
        [HttpGet("ByUser/{username}")]
        public async Task<ActionResult<IEnumerable<AppointmentBooking>>> GetBookingsByUser(string username)
        {
            var bookings = await _context.AppointmentBookings.Where(b => b.UserName == username).ToListAsync();
            return bookings;
        }

        [HttpGet("Invoice/{id}")]
        public async Task<IActionResult> GenerateInvoice(int id)
        {
            var booking = await _context.AppointmentBookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            // Create PDF document
            var document = new PdfDocument();
            var page = document.AddPage();
            var graphics = XGraphics.FromPdfPage(page);
            var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
            var fontHeading = new XFont("Arial", 14, XFontStyle.Bold);
            var fontNormal = new XFont("Arial", 12, XFontStyle.Regular);

            // Define invoice title
            var title = "Invoice for Appointment Booking";

            // Draw title on PDF page
            graphics.DrawString(title, fontTitle, XBrushes.Black, new XRect(30, 30, page.Width.Point, 30), XStringFormats.TopLeft);

            // Draw invoice number
            graphics.DrawString($"Invoice Number: {id}", fontNormal, XBrushes.Black, new XRect(30, 70, page.Width.Point, 30), XStringFormats.TopLeft);

            // Draw booking details heading
            graphics.DrawString("Booking Details", fontHeading, XBrushes.Black, new XRect(30, 100, page.Width.Point, 30), XStringFormats.TopLeft);

           
            // Define booking details content
            var bookingDetails = $"Appointment ID: {booking.VetName}{Environment.NewLine}{Environment.NewLine}" +
                                 $"User Name: {booking.UserName}{Environment.NewLine}{Environment.NewLine}" +
                                 $"Booking Date: {booking.BookingDate:MM/dd/yyyy HH:mm}{Environment.NewLine}{Environment.NewLine}" + // Format date and time
                                 $"Price: {booking.Price:C}{Environment.NewLine}{Environment.NewLine}"; // Display price as currency


            // Draw booking details content on PDF page
            graphics.DrawString(bookingDetails, fontNormal, XBrushes.Black, new XRect(30, 130, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

            // Draw contact information
            graphics.DrawString("Contact Information", fontHeading, XBrushes.Black, new XRect(30, 200, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("www.vetAppointment.com", fontNormal, XBrushes.Black, new XRect(30, 230, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("7 jhapa", fontNormal, XBrushes.Black, new XRect(30, 250, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("nepal, provice 1,23322", fontNormal, XBrushes.Black, new XRect(30, 270, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("Phone: 98132981321", fontNormal, XBrushes.Black, new XRect(30, 290, page.Width.Point, 30), XStringFormats.TopLeft);
            graphics.DrawString("Email: verAppointment@example.com", fontNormal, XBrushes.Black, new XRect(30, 310, page.Width.Point, 30), XStringFormats.TopLeft);

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