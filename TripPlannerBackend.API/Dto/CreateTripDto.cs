namespace TripPlannerBackend.API.Dto
{
    public class CreateTripDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
 