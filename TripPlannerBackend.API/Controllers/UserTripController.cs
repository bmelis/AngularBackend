using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.API.Services;
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
        private readonly TripAuthorizationService _tripAuthorizationService;
        public UserTripController(TripPlannerDbContext context, IMapper mapper, TripAuthorizationService tripAuthorizationService)
        {
            _context = context;
            _mapper = mapper;
            _tripAuthorizationService = tripAuthorizationService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<List<GetUserTripDto>>> Create([FromBody] List<GetUserTripDto> createUserTripDtos, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.GetUserRole(tripId, email) != "admin") return StatusCode(403);

            List<UserTrip> userTripsToConvert = new List<UserTrip>();
            int count = 0;
            try
            {
                foreach (GetUserTripDto createUserTripDto in createUserTripDtos)
                {
                    if (createUserTripDto.RoleId == 1) throw new Exception();

                    UserTrip userTripToAdd = _mapper.Map<UserTrip>(createUserTripDto);
                    _context.UserTrips.Add(userTripToAdd);
                    userTripsToConvert.Add(userTripToAdd);
                    count++;
                }
            }
            catch (Exception e)
            {
                if (count > 0)
                {
                    await _context.SaveChangesAsync();
                }
                return BadRequest();
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), new { }, _mapper.Map<List<GetUserTripDto>>(userTripsToConvert));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserTripDto>> GetById([FromRoute] int id)
        {
            var userTrip = await _context.UserTrips.FindAsync(id);
            if (userTrip == null) return NotFound();

            return _mapper.Map<GetUserTripDto>(userTrip);
        }

        [Authorize]
        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<List<GetUserTripDto>>> GetByTripId([FromRoute] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.GetUserRole(tripId, email) != "admin") return StatusCode(403);

            List<UserTrip> userTrips = await _context.UserTrips
                .Where(ut => ut.TripId == tripId)
                .ToListAsync();
            if (!userTrips.Any()) return NotFound();

            return _mapper.Map<List<GetUserTripDto>>(userTrips);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GetUserTripDto>>> GetAll()
        {
            var userTrips = await _context.UserTrips.ToListAsync();
            if (!userTrips.Any()) return NotFound();

            return _mapper.Map<List<GetUserTripDto>>(userTrips);
        }
        
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] List<GetUserTripDto> updateUserTripDtos, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.GetUserRole(tripId, email) != "admin") return StatusCode(403);

            int count = 0;
            try
            {
                foreach (GetUserTripDto updateUserTripDto in updateUserTripDtos)
                {
                    if (updateUserTripDto.RoleId == 1) throw new Exception();

                    UserTrip? userTrip = await _context.UserTrips.FindAsync(updateUserTripDto.Id);
                    if (userTrip == null) return NotFound();
                    _mapper.Map(updateUserTripDto, userTrip);
                    count++;
                }
            } catch (Exception e) 
            {
                if (count > 0)
                {
                    await _context.SaveChangesAsync();
                }
                return BadRequest();
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] List<int> ids, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.GetUserRole(tripId, email) != "admin") return StatusCode(403);

            int count = 0;
            try
            {
                foreach (int id in ids)
                {
                    UserTrip? userTrip = await _context.UserTrips.FindAsync(id);
                    if (userTrip == null) return NotFound();

                    _context.UserTrips.Remove(userTrip);
                    count++;
                }                
            }
            catch (Exception e)
            {
                if (count > 0)
                {
                    await _context.SaveChangesAsync();
                }
                return BadRequest();
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
