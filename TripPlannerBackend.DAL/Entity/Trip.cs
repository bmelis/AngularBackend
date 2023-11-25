using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlannerBackend.DAL.Entity
{
    public class Trip
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string ?Description { get; set; }
        public bool IsPublic { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }

        public ICollection<SavedTrip> ?SavedTrips { get; set; }
        public ICollection<UserTrip> ?UserTrips { get; set; }
        public ICollection<Destination> ?Destinations { get; set; }
    }
}