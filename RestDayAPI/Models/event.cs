
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        public int OrganiserID { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime EventDate { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Distance { get; set; }

        [MaxLength(50)]
        public string? EventType { get; set; } // "Run", "Walk", "Cycle"

        [ForeignKey("OrganiserID")]
        public User? Organiser { get; set; }

        public ICollection<Category>? Categories { get; set; }
        public ICollection<Route>? Routes { get; set; }
    }
}
