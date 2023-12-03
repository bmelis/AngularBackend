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
    public class AccommodationController : ControllerBase
    {
        private readonly TripPlannerDbContext _context;
        private readonly IMapper _mapper;
        private TripAuthorizationService tripAuthorizationService;
        public AccommodationController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            tripAuthorizationService = new TripAuthorizationService(context);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GetAccommodationDto>> Create([FromBody] CreateAccommodationDto createAccommodationDto, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            Accommodation accommodationToAdd = _mapper.Map<Accommodation>(createAccommodationDto);
            await _context.Accommodations.AddAsync(accommodationToAdd);
            await _context.SaveChangesAsync();
            GetAccommodationDto accommodationToReturn = _mapper.Map<GetAccommodationDto>(accommodationToAdd);

            return CreatedAtAction(nameof(Create), new { id = accommodationToReturn.Id }, accommodationToReturn);
        }

        [HttpGet("{destinationId}")]
        [Authorize]
        [HttpGet("destination/{destinationId}")]
        public async Task<ActionResult<List<GetAccommodationDto>>> GetByDestinationId([FromRoute] int destinationId, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            List<Accommodation> accommodations = await _context.Accommodations.Where(a => a.DestinationId == destinationId).ToListAsync();
            if (!accommodations.Any()) return NotFound();

            return _mapper.Map<List<GetAccommodationDto>>(accommodations);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<GetAccommodationDto>> Update([FromRoute] int id, [FromBody] CreateAccommodationDto updateAccommodationDto, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            Accommodation? existingAccommodation = await _context.Accommodations.FindAsync(id);
            if (existingAccommodation == null) return NotFound();

            Accommodation updatedAccommodation = _mapper.Map(updateAccommodationDto, existingAccommodation);
            await _context.SaveChangesAsync();
            GetAccommodationDto getAccommodationDto = _mapper.Map<GetAccommodationDto>(updatedAccommodation);

            return Ok(getAccommodationDto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete([FromRoute] int id, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            Accommodation? accommodation = await _context.Accommodations.FindAsync(id);
            if (accommodation == null) return NotFound();

            _context.Accommodations.Remove(accommodation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
