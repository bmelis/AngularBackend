using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlannerBackend.DAL.Entity
{
    public class AccommodationType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Accommodation> ?Accommodations { get; set; }
    }
}