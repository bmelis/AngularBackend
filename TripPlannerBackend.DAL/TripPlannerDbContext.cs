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
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Destinations)
                .WithOne(d => d.Trip)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Destination>()
                .HasMany(d => d.Accommodations)
                .WithOne(a => a.Destination)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Destination>()
                .HasMany(d => d.Activities)
                .WithOne(a => a.Destination)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivityType>()
                .HasMany(at => at.Activities)
                .WithOne(a => a.ActivityType)
                .HasForeignKey(a => a.ActivityTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccommodationType>()
                .HasMany(at => at.Accommodations)
                .WithOne(a => a.AccommodationType)
                .HasForeignKey(a => a.AccommodationTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Accommodation>().ToTable("Accommodation");
            modelBuilder.Entity<AccommodationType>().ToTable("AccommodationType");
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