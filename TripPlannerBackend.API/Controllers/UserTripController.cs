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

        [HttpPost]
        public async Task<ActionResult<GetUserTripDto>> Create([FromBody] GetUserTripDto getUserTripDto)
        {
            UserTrip userTripToAdd = _mapper.Map<UserTrip>(getUserTripDto);
            _context.UserTrips.Add(userTripToAdd);
            await _context.SaveChangesAsync();
            UserTrip userTripToReturn = _mapper.Map<UserTrip>(userTripToAdd);

            return CreatedAtAction(nameof(Create), new { id = userTripToReturn.UserTripId }, userTripToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserTripDto>> GetById(int id)
        {
            var userTrip = await _context.UserTrips.FindAsync(id);
            if (userTrip == null) return NotFound();

            return _mapper.Map<GetUserTripDto>(userTrip);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetUserTripDto>>> GetAll()
        {
            var userTrips = await _context.UserTrips.ToListAsync();
            if (!userTrips.Any()) return NotFound();

            return _mapper.Map<List<GetUserTripDto>>(userTrips);
        }              
    }
}
