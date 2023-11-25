using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.DAL.Initializer
{
    public class DBInitializer
    {
        public static void Initialize(TripPlannerDbContext context)
        {
            context.Database.EnsureCreated();
            /*------------------------------------------*/

            if (context.Trips.Any())
            {
                return;
            }

            context.Trips.AddRange(
                new Trip {
                    Name = "Trip 1",
                    Description = "test description",
                    IsPublic = false,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7),
                },
                new Trip
                {
                    Name = "Trip 2",
                    Description = "test description",
                    IsPublic = false,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7),
                }
                ) ;
            context.SaveChanges();

            /*------------------------------------------*/

            context.SavedTrips.AddRange(
                new SavedTrip { UserId = "bent.melis@gmail.com", TripId = 1}
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.Roles.AddRange(
                new Role { Name = "admin" },
                new Role { Name = "editor" },
                new Role { Name = "regular" }
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.UserTrips.AddRange(
                new UserTrip { UserId = "bent.melis@gmail.com", TripId = 1, RoleId = 1}
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.Destinations.AddRange(
                new Destination { Country = "Belgium", City = "Mol", TripId = 1 }
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.ActivityTypes.AddRange(
                new ActivityType { Name = "Sport" },
                new ActivityType { Name = "Cultuur" }
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.Activities.AddRange(
                new Activity { Title = "Darten", Description = "Darten in het hotel", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(2), DestinationId = 1, ActivityTypeId = 1 },
                new Activity { Title = "Zwemmen", Description = "We verzamelen aan het hotel", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(1), DestinationId = 1, ActivityTypeId = 1 },
                new Activity { Title = "Museum", Description = "Leerrijke uitstap naar een museum.", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(4), DestinationId = 1, ActivityTypeId = 2 },
                new Activity { Title = "Darten", Description = "Darten in het hotel", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(2), DestinationId = 1, ActivityTypeId = 1 }
                );
            context.SaveChanges();           

            /*------------------------------------------*/

            context.AccommodationTypes.AddRange(
                new AccommodationType { Name = "Bungaloo" },
                new AccommodationType { Name = "Hotel" }
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.Accommodations.AddRange(
                new Accommodation { Name = "beunhaas bungaloo", Street = "Haag", HouseNumber = "65", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7), DestinationId = 1, AccommodationTypeId = 1 }
                );
            context.SaveChanges();
        }
    }
}

