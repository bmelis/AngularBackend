namespace TripPlannerBackend.API.Dto
{
    public class GetAccommodationDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string? Mailbox { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DestinationId { get; set; }
        public int AccommodationTypeId { get; set; }

        public GetDestinationDto Destination { get; set; }
        public GetAccommodationTypeDto AccommodationType { get; set; }
    }
}
