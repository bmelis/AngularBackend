using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.API.Services;
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
        private readonly TripAuthorizationService _tripAuthorizationService;
        public TripController(TripPlannerDbContext context, IMapper mapper, TripAuthorizationService tripAuthorizationService)
        {
            _context = context;
            _mapper = mapper;
            _tripAuthorizationService = tripAuthorizationService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateTripDto trip)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            // Create trip
            Trip tripToAdd = _mapper.Map<Trip>(trip);
            _context.Trips.Add(tripToAdd);
            await _context.SaveChangesAsync();
            GetTripDto tripToReturn = _mapper.Map<GetTripDto>(tripToAdd);

            //Create initial UserTrip for admin (creator)
            UserTrip userTrip = new UserTrip();
            userTrip.TripId = tripToReturn.Id;
            userTrip.UserId = email;
            userTrip.RoleId = 1;
            _context.UserTrips.Add(userTrip);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = tripToReturn.Id }, tripToReturn.Id);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTripDto>> GetById([FromRoute] int id)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            Trip? trip = await _context.Trips
                .Include(t => t.Destinations)
                    .ThenInclude(d => d.Activities)
                .Include(t => t.Destinations)
                    .ThenInclude(d => d.Accommodations)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (trip == null) return NotFound();

            GetTripDto tripDto = _mapper.Map<GetTripDto>(trip);
            string? role = await _tripAuthorizationService.GetUserRole(id, email);
            if (role == null)
            {
                if (trip.IsPublic) return tripDto;
                return StatusCode(403, "Forbidden: You do not have access to this resource.");    
            }
            tripDto.UserRole = role;
            return tripDto;
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<List<GetTripDto>>> GetByKeyword([FromQuery] string keyword)
        {
            var trips = await _context.Trips
                .Where(t => t.IsPublic == true)
                .Where(t => EF.Functions.Like(t.Name, $"%{keyword}%") || EF.Functions.Like(t.Description, $"%{keyword}%"))
                .ToListAsync();

            if (!trips.Any())
            {
                return Ok("No trips found matching the provided key.");
            }

            return _mapper.Map<List<GetTripDto>>(trips);                   
        }

        [Authorize]
        [HttpGet("email")]
        public async Task<ActionResult<List<GetTripDto>>> GetByEmail()
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            var trips = await _context.Trips
                .Include(t => t.UserTrips)
                .ThenInclude(ut => ut.Role)
                .Where(t => t.UserTrips.Any(ut => ut.UserId == email))
                .ToListAsync();

            if (!trips.Any())
            {
                return Ok("no trips found");
            }

            List<GetTripDto> tripDtos = _mapper.Map<List<GetTripDto>>(trips);
            for (int i = 0; i < trips.Count; i++)
            {
                string? role = trips[i].UserTrips.First(ut => ut.UserId == email).Role.Name;
                tripDtos[i].UserRole = role;
            }

            return tripDtos;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GetTripDto>>> GetAll()
        {
            var trips = await _context.Trips.Where(t => t.IsPublic == true).ToListAsync();
            if (!trips.Any()) return NotFound();

            return _mapper.Map<List<GetTripDto>>(trips);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Trip>> Update([FromRoute] int id, [FromBody] CreateTripDto updateTripDto)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            string? role = await _tripAuthorizationService.GetUserRole(id, email);
            if (role != "admin") return StatusCode(403);

            Trip? trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();

            Trip updatedTrip = _mapper.Map(updateTripDto, trip);
            await _context.SaveChangesAsync();
            GetTripDto getTripDto = _mapper.Map<GetTripDto>(updatedTrip);

            getTripDto.UserRole = role;
            return Ok(getTripDto);
        }

        [Authorize]
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
