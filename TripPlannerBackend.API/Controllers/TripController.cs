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
    public class TripController : ControllerBase
    {
        private readonly TripPlannerDbContext _context;
        private readonly IMapper _mapper;
        public TripController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Get By ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTripDto>> GetTrip(int id)
        {
            var trip = await _context.Trips.SingleAsync(t => t.Id == id);

            if (trip == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetTripDto>(trip);
        }


        //Get By ID
        [HttpGet("email/{email}")]
        public async Task<ActionResult<List<GetTripDto>>> GetTrip(String email)
        {
            var trip = await _context.Trips
                .Include(t => t.UserTrips)
                .Where(t => t.UserTrips.Any(ut => ut.UserId == email))
                .ToListAsync();

            if (trip == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<GetTripDto>>(trip);
        }

        // Get ALL
        [HttpGet]
        public async Task<ActionResult<List<GetTripDto>>> GetTrips()
        {
            var trips = await _context.Trips.Include(t => t.Activities).ToListAsync();

            if (trips == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<GetTripDto>>(trips);
        }

        //Insert - you have to be authenticated
        [HttpPost]
        public async Task<ActionResult<GetTripDto>> AddTrip(CreateTripDto trip)
        {
            //We map the CreateTripDto to the Trip entity object
            Trip tripToAdd = _mapper.Map<Trip>(trip);
            _context.Trips.Add(tripToAdd);
            await _context.SaveChangesAsync();
            GetTripDto tripToReturn = _mapper.Map<GetTripDto>(tripToAdd);

            return CreatedAtAction(nameof(GetTrip), new { id = tripToReturn.Id }, tripToReturn);
        }
    }
}
