using System.ComponentModel.DataAnnotations;

namespace VetAppoinment.Models.Entities
{
    public class MedicineBooking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public string? MedicineName { get; set; }
        [Required]
        public string? UserName { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        [EmailAddress]
        public string? Email { get; set; }
        public double  Price { get; set; }
        public string? Image { get; set; }

    }
}
