using Microsoft.EntityFrameworkCore;
using TripPlannerBackend.DAL;
using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.API.Services
{
    public class TripAuthorizationService
    {
        private readonly TripPlannerDbContext _context;


        public TripAuthorizationService(TripPlannerDbContext context) {
            _context = context;
        }

        public async Task<bool> IsParticipantOrNull(int tripId, string email)
        {
            string? role = await GetUserRole(tripId, email);
            if (role == null || role == "participant") return true;
            return false;
        }

        public async Task<string?> GetUserRole(int tripId, string email)
        {
            Trip? trip = await _context.Trips
                .Include(t => t.Destinations)
                    .ThenInclude(d => d.Activities)
                .Include(t => t.Destinations)
                    .ThenInclude(d => d.Accommodations)
                .Include(t => t.UserTrips)
                    .ThenInclude(ut => ut.Role)
                .SingleOrDefaultAsync(t => t.Id == tripId);

            UserTrip? userTrip = trip.UserTrips.FirstOrDefault(ut => ut.UserId == email);
            if (userTrip == null) return null;

            return userTrip.Role.Name;
        }
    }
}
