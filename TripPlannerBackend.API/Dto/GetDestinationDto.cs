using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.API.Dto
{
    public class GetDestinationDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int TripId { get; set; }

        public Trip Trip { get; set; }
        public IEnumerable<GetAccommodationDto> Accommodations { get; set; }
    }
}
