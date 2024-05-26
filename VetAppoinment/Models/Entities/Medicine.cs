using System.ComponentModel.DataAnnotations;

namespace VetAppoinment.Models.Entities
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ?Name { get; set; }

        [Required]
        public string ?Description { get; set; }
        [EmailAddress]
        public string ?Email { get; set; }
        
        public double Price { get; set; }

        public string? Image {  get; set; }
    }
}
