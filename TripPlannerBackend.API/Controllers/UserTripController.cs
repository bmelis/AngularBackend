using AutoMapper;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.DAL;
using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTripController : ControllerBase
    {
        private readonly TripPlannerDbContext _context;
        private readonly IMapper _mapper;
        public UserTripController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetUserTripDto>>> GetUserTrips()
        {
            var trips = await _context.UserTrips.ToListAsync();

            if (trips == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<GetUserTripDto>>(trips);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserTripDto>> GetUserTrip(int id)
        {
            var userTrip = await _context.UserTrips.SingleAsync(t => t.UserTripId == id);

            if (userTrip == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetUserTripDto>(userTrip);
        }

        [HttpPost]
        public async Task<ActionResult<GetUserTripDto>> AddUserTrip(GetUserTripDto usertTrip)
        {
            UserTrip tripToAdd = _mapper.Map<UserTrip>(usertTrip);
            _context.UserTrips.Add(tripToAdd);
            await _context.SaveChangesAsync();
            UserTrip tripToReturn = _mapper.Map<UserTrip>(tripToAdd);

            return CreatedAtAction(nameof(GetUserTrip), new { id = tripToReturn.UserTripId }, tripToReturn);
        }
    }
}
