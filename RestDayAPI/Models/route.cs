
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    public class Route
    {
        [Key]
        public int RouteID { get; set; }

        public int? EventID { get; set; }

        [Required, MaxLength(100)]
        public string RouteName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,2)")]
        public decimal Distance { get; set; }

        public string? Description { get; set; }

        [ForeignKey("EventID")]
        public Event? Event { get; set; }
    }
}