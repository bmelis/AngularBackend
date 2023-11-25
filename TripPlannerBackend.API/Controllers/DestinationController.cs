using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlannerBackend.API.Dto;
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
        public DestinationController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<GetDestinationDto>> Create([FromBody] CreateDestinationDto createDestinationDto)
        {
            Destination destination = _mapper.Map<Destination>(createDestinationDto);
            await _context.Destinations.AddAsync(destination);
            await _context.SaveChangesAsync();
            GetDestinationDto getDestinationDto = _mapper.Map<GetDestinationDto>(destination);

            return CreatedAtAction(nameof(Create), new { id = getDestinationDto.Id }, getDestinationDto);
        }

        [HttpGet("{tripId}")]
        public async Task<ActionResult<List<GetDestinationDto>>> GetByTripId(int tripId)
        {
            List<Destination> destinations = await _context.Destinations.Where(d => d.TripId == tripId).ToListAsync();
            if (destinations == null) return NotFound();

            return _mapper.Map<List<GetDestinationDto>>(destinations);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetDestinationDto>> Update(int id, [FromBody] CreateDestinationDto updateDestinationDto)
        {
            Destination? destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            Destination updatedDestination = _mapper.Map(updateDestinationDto, destination);
            await _context.SaveChangesAsync();
            GetDestinationDto getDestinationDto = _mapper.Map<GetDestinationDto>(updatedDestination);

            return CreatedAtAction(nameof(Update), new { id = id }, getDestinationDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Destination? destination = await _context.Destinations.FindAsync(id);

            if (destination == null) return NotFound();

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
