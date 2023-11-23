namespace TripPlannerBackend.API.Dto
{
    public class GetUserTripDto
    {   
        public int UserTripId { get; set; }
        public String UserId { get; set; }
        public int TripId { get; set; }
        public int RoleId { get; set; }

    }
}
