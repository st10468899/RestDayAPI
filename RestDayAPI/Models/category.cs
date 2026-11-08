using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public int EventID { get; set; }

        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? CategoryType { get; set; }

        public string? Description { get; set; }

        [ForeignKey("EventID")]
        public Event? Event { get; set; }

        public ICollection<Enrolment>? Enrolments { get; set; }
    }
}