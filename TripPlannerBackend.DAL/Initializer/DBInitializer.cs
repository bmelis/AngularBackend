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
                    Name = "De kempen verkennen",
                    Description = "Met de familiedoor de kempen trekken!",
                    IsPublic = false,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7),
                },
                new Trip
                {
                    Name = "Barcelona",
                    Description = "test description",
                    IsPublic = true,
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
                new Role { Name = "participant" }
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.UserTrips.AddRange(
                new UserTrip { UserId = "bent.melis@gmail.com", TripId = 1, RoleId = 1 },
                new UserTrip { UserId = "xeyocab737@dpsols.com", TripId = 1, RoleId = 2 },
                new UserTrip { UserId = "seppem2@gmail.com", TripId = 1, RoleId = 3 },
                new UserTrip { UserId = "xeyocab737@dpsols.com", TripId = 2, RoleId = 1 },
                new UserTrip { UserId = "peetersbrent@gmail.com", TripId = 1, RoleId = 2 },
                new UserTrip { UserId = "michielvanloy2003@gmail.com", TripId = 1, RoleId = 2 }
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.Destinations.AddRange(
                new Destination { Country = "België", City = "Mol", ImageUrl = "/assets/images/trip-placeholders/1.png", TripId = 1 },
                new Destination { Country = "België", City = "Geel", ImageUrl = "/assets/images/trip-placeholders/2.png", TripId = 1 },
                new Destination { Country = "Nederland", City = "Amsterdam", ImageUrl = "/assets/images/trip-placeholders/2.png", TripId = 1 }
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.ActivityTypes.AddRange(
                new ActivityType { Name = "Evenement" },
                new ActivityType { Name = "Sport" },
                new ActivityType { Name = "Cultuur" }                
                );
            context.SaveChanges();

            /*------------------------------------------*/

            context.Activities.AddRange(
                new Activity { Title = "Darten", Description = "Darten in het hotel", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(2), DestinationId = 1, ActivityTypeId = 2 },
                new Activity { Title = "Zwemmen", Description = "We verzamelen aan het hotel", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(1), DestinationId = 1, ActivityTypeId = 2 },
                new Activity { Title = "Museum", Description = "Leerrijke uitstap naar een museum.", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(4), DestinationId = 1, ActivityTypeId = 3 },
                new Activity { Title = "Darten", Description = "Darten in het hotel", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(2), DestinationId = 1, ActivityTypeId = 2 }
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
                new Accommodation { Name = "bungaloo", Street = "Haag", HouseNumber = "65", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7), DestinationId = 1, AccommodationTypeId = 1 },
                new Accommodation { Name = "Hotel in Geel", Street = "langestraat", HouseNumber = "12", StartDate = new DateTime(2024, 10, 30, 12, 0, 0), EndDate = new DateTime(2024, 11, 2, 12, 0, 0), DestinationId = 2, AccommodationTypeId = 2 }
                );
            context.SaveChanges();
        }
    }
}

