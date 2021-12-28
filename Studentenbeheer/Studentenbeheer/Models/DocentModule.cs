using System.ComponentModel.DataAnnotations.Schema;

namespace Studentenbeheer.Models
{
    public class DocentModule
    {
        public int Id { get; set; }
        [ForeignKey("ModuleId")]
        public int ModuleId { get; set; }
        public Module? Module { get; set; }

        [ForeignKey("DocentId")]
        public int DocentId { get; set; }
        public Docent? Docent { get; set; }
    }
}
