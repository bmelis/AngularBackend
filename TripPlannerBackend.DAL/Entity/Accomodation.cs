using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlannerBackend.DAL.Entity
{
    public class Accomodation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string HouseNumber { get; set; }
        public string ?Mailbox { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public int DestinationId { get; set; }
        public int AccomadationTypeId { get; set; }

        public Destination Destination { get; set; }
        public AccomodationType AccomodationType { get; set; }
    }
}