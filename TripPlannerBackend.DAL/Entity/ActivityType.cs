using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlannerBackend.DAL.Entity
{
    public class ActivityType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Activity> ?Activities { get; set; }
    }
}