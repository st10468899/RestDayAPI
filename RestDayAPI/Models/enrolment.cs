
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    public class Enrolment
    {
        [Key]
        public int EnrolmentID { get; set; }

        public int ParticipantID { get; set; }

        public int CategoryID { get; set; }

        public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("ParticipantID")]
        public User? Participant { get; set; }

        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }

        public Result? Result { get; set; }
    }
}
