namespace TripPlannerBackend.API.Dto
{
    public class CreateDestinationDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public int TripId { get; set; }
    }
}
