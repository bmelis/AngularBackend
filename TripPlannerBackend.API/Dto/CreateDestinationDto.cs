namespace TripPlannerBackend.API.Dto
{
    public class CreateDestinationDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string? ImageUrl { get; set; }
        public int TripId { get; set; }
    }
}
