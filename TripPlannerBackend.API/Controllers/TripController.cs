using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

        //Insert - you have to be authenticated
        [HttpPost]
        public async Task<ActionResult<GetTripDto>> Create([FromBody] CreateTripDto trip)
        {
            //We map the CreateTripDto to the Trip entity object
            Trip tripToAdd = _mapper.Map<Trip>(trip);
            _context.Trips.Add(tripToAdd);
            await _context.SaveChangesAsync();
            GetTripDto tripToReturn = _mapper.Map<GetTripDto>(tripToAdd);

            return CreatedAtAction(nameof(Create), new { id = tripToReturn.Id }, tripToReturn);
        }        

        //Get By ID
        [HttpGet("id")]
        public async Task<ActionResult<GetTripDto>> GetById(int id)
        {
            Trip? trip = await _context.Trips
                .Include(t => t.Destinations)
                    .ThenInclude(d => d.Activities)
                .Include(t => t.Destinations)
                    .ThenInclude(d => d.Accommodations)
                .SingleOrDefaultAsync(t => t.Id == id);            

            if (trip == null) return NotFound();

            return _mapper.Map<GetTripDto>(trip);
        }


        //Get By ID
        [HttpGet("email/{email}")]
        public async Task<ActionResult<List<GetTripDto>>> GetByEmail(String email)
        {
            var trip = await _context.Trips
                .Include(t => t.UserTrips)
                .Where(t => t.UserTrips.Any(ut => ut.UserId == email))
                .ToListAsync();

            if (!trip.Any()) return NotFound();

            return _mapper.Map<List<GetTripDto>>(trip);
        }

        // Get ALL
        [HttpGet]
        public async Task<ActionResult<List<GetTripDto>>> GetAll()
        {
            var trips = await _context.Trips.ToListAsync();
            if (!trips.Any()) return NotFound();

            return _mapper.Map<List<GetTripDto>>(trips);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Trip>> Update(int id, [FromBody] CreateTripDto updateTripDto)
        {
            Trip? trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();

            Trip updatedTrip = _mapper.Map(updateTripDto, trip);
            await _context.SaveChangesAsync();
            GetTripDto getTripDto = _mapper.Map<GetTripDto>(updatedTrip);

            return Ok(getTripDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Trip? trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
