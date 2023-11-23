namespace TripPlannerBackend.API.Dto
{
    public class GetActivityDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TripId { get; set; }
        public int ActivityTypeId { get; set; }
    }
}
