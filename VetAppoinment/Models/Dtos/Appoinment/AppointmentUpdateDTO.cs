using System.ComponentModel.DataAnnotations;

namespace VetAppoinment.Models.Dtos.Appoinment
{
    public class AppointmentUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? VetName { get; set; }

        [Required]
      
        public string? AppointmentDate { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
