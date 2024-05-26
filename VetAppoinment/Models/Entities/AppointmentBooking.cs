using System.ComponentModel.DataAnnotations;

namespace VetAppoinment.Models.Entities
{
    public class AppointmentBooking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        public string VetName { get; set; }

        public string BookStatus { get; set; }

        public string Image { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public double Price { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        [EmailAddress]
        public string? Email { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}
