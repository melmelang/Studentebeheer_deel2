using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentenbeheer.Models
{
    [Authorize(Roles = "Admin")]
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string? Voornaam { get; set; }
        [Required]
        public string? Achternaam { get; set; }

        [Required]
        [DataType (DataType.Date)]
        public DateTime Geboortedatum { get; set; }
        public DateTime? Deleted { get; set; } = DateTime.MaxValue;

        [ForeignKey("Gender")]
        public char GeslachtId { get; set; }
        public Gender? Geslacht { get; set; }

        [ForeignKey("User")]
        public string userId { get; set; }
        public Users? User;

    }

    public class StudentIndexViewModel
    {
        public string NaamFilter { get; set; }
        public char GeslachtIdFilter { get; set; }
        public List<Student> FilteredStudent { get; set; }
        public SelectList GenderToSelect { get; set; }
    }
}
