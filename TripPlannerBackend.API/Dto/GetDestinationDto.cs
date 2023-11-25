using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.API.Dto
{
    public class GetDestinationDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int TripId { get; set; }

        public IEnumerable<GetActivityDto> Activities { get; set; }
        public IEnumerable<GetAccommodationDto> Accommodations { get; set; }
    }
}
