using System.ComponentModel.DataAnnotations;

namespace VetAppoinment.Models.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? VetName { get; set; }//doctor name 
        public string AppointmentDate { get; set; }//date of appointment for doctor 10 a.m -4 pm 
        public string? Description { get; set; }
        public string? Image { get; set; }

        public double Price { get; set; }

        [EmailAddress]
        public string?Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

    }
}
