namespace TripPlannerBackend.API.Dto
{
    public class CreateActivityDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? Rating { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DestinationId { get; set; }
        public int ActivityTypeId { get; set; }
    }
}
