namespace RaceDayAPI.DTOs
{
    public class EventDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string? Location { get; set; }
        public decimal Distance { get; set; }
        public string? EventType { get; set; }
    }

    public class CategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryType { get; set; }
        public string? Description { get; set; }
    }
}