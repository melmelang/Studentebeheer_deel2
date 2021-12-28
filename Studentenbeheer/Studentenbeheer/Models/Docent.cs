using Studentenbeheer.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentenbeheer.Models
{
    public class Docent
    {
        public int Id { get; set; }
        [Required]
        public string? Voornaam { get; set; }
        [Required]
        public string? Achternaam { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Geboortedatum { get; set; }
        public DateTime? Deleted { get; set; } = DateTime.MaxValue;

        [ForeignKey("GenderId")]
        public char GeslachtId { get; set; }
        public Gender? Geslacht { get; set; }

        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

    }
}
