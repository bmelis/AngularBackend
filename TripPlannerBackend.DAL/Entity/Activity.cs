using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlannerBackend.DAL.Entity
{
    public class Activity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Title { get; set; }
        public string ?Description { get; set; }
        public int ?Rating { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public int TripId { get; set; }
        public int ActivityTypeId { get; set; }

        public Trip Trip { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}