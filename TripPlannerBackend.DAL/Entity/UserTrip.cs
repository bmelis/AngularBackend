using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlannerBackend.DAL.Entity
{
    public class UserTrip
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTripId { get; set; }
        public string UserId { get; set; }
        public int TripId { get; set; }
        public int RoleId { get; set; }
        public Trip Trip { get; set; }
        public Role Role { get; set; }
    }
}