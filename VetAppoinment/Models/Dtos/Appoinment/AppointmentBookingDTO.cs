using System.ComponentModel.DataAnnotations;

namespace VetAppoinment.Models.Dtos.Appoinment
{
    public class AppointmentBookingDTO
    {
        [Required]
        public int AppointmentId { get; set; }

        public string? BookStatus { get; set; }


        public string? VetName { get; set; }
        public string? Image { get; set; }

        public string? UserName { get; set; }

        public double Price { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
