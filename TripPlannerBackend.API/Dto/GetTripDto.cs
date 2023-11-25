namespace TripPlannerBackend.API.Dto
{
    public class GetTripDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<GetActivityDto> Activities { get; set; }
        public IEnumerable<GetDestinationDto> Destinations { get; set; }
    }
}
