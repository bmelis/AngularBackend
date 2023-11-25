using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlannerBackend.DAL.Entity
{
    public class Destination
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public int TripId { get; set; }

        public Trip Trip;
        public ICollection<Activity>? Activities { get; set; }
        public ICollection<Accommodation>? Accommodations { get; set; }
    }
}