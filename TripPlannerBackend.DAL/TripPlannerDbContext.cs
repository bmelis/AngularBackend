using Microsoft.EntityFrameworkCore;
using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.DAL
{
    public class TripPlannerDbContext : DbContext
    {
        public TripPlannerDbContext()
        {

        }

        public TripPlannerDbContext(DbContextOptions<TripPlannerDbContext> options) : base(options)
        {
        }
        public DbSet<Accommodation> Accommodations => Set<Accommodation>();
        public DbSet<AccommodationType> AccommodationTypes => Set<AccommodationType>();
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<ActivityType> ActivityTypes => Set<ActivityType>();
        public DbSet<Destination> Destinations => Set<Destination>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<SavedTrip> SavedTrips => Set<SavedTrip>();
        public DbSet<Trip> Trips => Set<Trip>();
        public DbSet<UserTrip> UserTrips => Set<UserTrip>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<User>()
            //     .HasMany(e => e.SavedTrips)
            //     .WithOne(e => e.User)
            //     .IsRequired();
            // modelBuilder.Entity<SavedTrip>()
            //     .HasOne(e => e.User)
            //     .WithMany(e => e.SavedTrips);

            // modelBuilder.Entity<Trip>()
            //    .HasMany(e => e.Activities)
            //    .WithOne(e => e.Trip)
            //    .HasForeignKey(e => e.TripId)
            //    .IsRequired();

            // modelBuilder.Entity<Activity>()
            //    .HasOne(e => e.Trip)
            //    .WithMany(e => e.Activities)
            //    .HasForeignKey(e => e.TripId)
            //    .IsRequired();

            modelBuilder.Entity<Accommodation>().ToTable("Accomadation");
            modelBuilder.Entity<AccommodationType>().ToTable("AccomadationType");
            modelBuilder.Entity<Activity>().ToTable("Activity");
            modelBuilder.Entity<ActivityType>().ToTable("ActivityType");
            modelBuilder.Entity<Destination>().ToTable("Destination");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<SavedTrip>().ToTable("SavedTrip");
            modelBuilder.Entity<Trip>().ToTable("Trip");
            modelBuilder.Entity<UserTrip>().ToTable("UserTrip");
        }
    }
}