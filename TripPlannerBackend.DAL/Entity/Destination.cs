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

        public Trip trip;
        public ICollection<Accomodation> ?Accomadations { get; set; }
    }
}