namespace RaceDayAPI.DTOs
{
    public class CreateEnrolmentDto
    {
        public int CategoryId { get; set; }
    }

    public class RecordResultDto
    {
        public int ParticipantId { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int FinishingPosition { get; set; }
    }
}