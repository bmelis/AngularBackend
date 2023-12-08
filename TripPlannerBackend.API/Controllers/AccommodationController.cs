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
        private readonly TripAuthorizationService _tripAuthorizationService;
        public AccommodationController(TripPlannerDbContext context, IMapper mapper, TripAuthorizationService tripAuthorizationService)
        {
            _context = context;
            _mapper = mapper;
            _tripAuthorizationService = tripAuthorizationService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GetAccommodationDto>> Create([FromBody] CreateAccommodationDto createAccommodationDto, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            DateTime accEndDate = createAccommodationDto.EndDate;
            DateTime endDate = new DateTime(accEndDate.Year, accEndDate.Month, accEndDate.Day, 23, 59, 59);
            createAccommodationDto.EndDate = endDate;

            Accommodation accommodationToAdd = _mapper.Map<Accommodation>(createAccommodationDto);
            await _context.Accommodations.AddAsync(accommodationToAdd);
            await _context.SaveChangesAsync();
            GetAccommodationDto accommodationToReturn = _mapper.Map<GetAccommodationDto>(accommodationToAdd);

            return CreatedAtAction(nameof(Create), new { id = accommodationToReturn.Id }, accommodationToReturn);
        }

        [Authorize]
        [HttpGet("destination/{destinationId}")]
        public async Task<ActionResult<List<GetAccommodationDto>>> GetByDestinationId([FromRoute] int destinationId, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            List<Accommodation> accommodations = await _context.Accommodations.Where(a => a.DestinationId == destinationId).ToListAsync();
            if (!accommodations.Any()) return Ok("er zijn nog geen accomodaties");

            return _mapper.Map<List<GetAccommodationDto>>(accommodations);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<GetAccommodationDto>> Update([FromRoute] int id, [FromBody] CreateAccommodationDto updateAccommodationDto, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            Accommodation? existingAccommodation = await _context.Accommodations.FindAsync(id);
            if (existingAccommodation == null) return NotFound();

            DateTime accEndDate = updateAccommodationDto.EndDate;
            DateTime endDate = new DateTime(accEndDate.Year, accEndDate.Month, accEndDate.Day, 23, 59, 59);
            updateAccommodationDto.EndDate = endDate;

            Accommodation updatedAccommodation = _mapper.Map(updateAccommodationDto, existingAccommodation);
            await _context.SaveChangesAsync();
            GetAccommodationDto getAccommodationDto = _mapper.Map<GetAccommodationDto>(updatedAccommodation);

            return Ok(getAccommodationDto);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            Accommodation? accommodation = await _context.Accommodations.FindAsync(id);
            if (accommodation == null) return NotFound();

            // Delete activities 'linked' to accommodation
            DateTime startDate = accommodation.StartDate;
            DateTime endDate = accommodation.EndDate;
            List<Activity> destinationActivities = _context.Activities.Where(a => a.DestinationId == accommodation.DestinationId).ToList();
            List<Activity> filteredActivities = destinationActivities
                .Where(a => 
                (a.StartDate >= startDate && a.StartDate <= endDate) 
                && 
                (a.EndDate >= startDate && a.EndDate <= endDate)).ToList();

            if (filteredActivities.Count > 0)
            {
                foreach (Activity activity in filteredActivities)
                {
                    _context.Activities.Remove(activity);
                }
            }

            _context.Accommodations.Remove(accommodation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
