using System.ComponentModel.DataAnnotations;

namespace VetAppoinment.Models.Dtos.Medicine
{
    public class MedicineUpdateDTO
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public double Price { get; set; }

        public string? Image { get; set; }
    }
}
