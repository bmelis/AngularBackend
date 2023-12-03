using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.API.Services;
using TripPlannerBackend.DAL;
using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly TripPlannerDbContext _context;
        private readonly IMapper _mapper;
        private TripAuthorizationService tripAuthorizationService;
        public DestinationController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            tripAuthorizationService = new TripAuthorizationService(context);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GetDestinationDto>> Create([FromBody] CreateDestinationDto createDestinationDto, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            Destination destinationToAdd = _mapper.Map<Destination>(createDestinationDto);
            await _context.Destinations.AddAsync(destinationToAdd);
            await _context.SaveChangesAsync();
            GetDestinationDto destinationToReturn = _mapper.Map<GetDestinationDto>(destinationToAdd);

            return CreatedAtAction(nameof(Create), new { id = destinationToReturn.Id }, destinationToReturn);
        }

        [Authorize]
        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<List<GetDestinationDto>>> GetByTripId([FromRoute] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            List<Destination> destinations = await _context.Destinations.Where(d => d.TripId == tripId).ToListAsync();
            if (destinations == null) return NotFound();

            return _mapper.Map<List<GetDestinationDto>>(destinations);
        }
        
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<GetDestinationDto>> Update([FromRoute] int id, [FromBody] CreateDestinationDto updateDestinationDto, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            Destination? destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            Destination updatedDestination = _mapper.Map(updateDestinationDto, destination);
            await _context.SaveChangesAsync();
            GetDestinationDto getDestinationDto = _mapper.Map<GetDestinationDto>(updatedDestination);

            return Ok(getDestinationDto);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            Destination? destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
