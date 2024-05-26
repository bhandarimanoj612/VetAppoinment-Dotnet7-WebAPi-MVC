using System.ComponentModel.DataAnnotations;

namespace VetAppoinment.Models.Dtos.Medicine
{
    public class MedicineBookingDTO
    {

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public string? MedicineName { get; set; }
        [Required]
        public string? UserName { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public string? Email { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
    }
}
