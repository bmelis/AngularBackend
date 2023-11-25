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
    public class AccommodationTypeController : ControllerBase
    {
        private readonly TripPlannerDbContext _context;
        private readonly IMapper _mapper;

        public AccommodationTypeController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // get accomidation type by id
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAccommodationTypeDto>> GetAccommodationType(int id)
        {
            var accommodationType = await _context.AccommodationTypes.SingleAsync(a => a.Id == id);

            if (accommodationType == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetAccommodationTypeDto>(accommodationType);
        }

        // get all accommodationtypes
        [HttpGet]
        public async Task<ActionResult<List<GetAccommodationTypeDto>>> GetAccommodationTypes()
        {
            var accommodationTypes = await _context.AccommodationTypes.ToListAsync();

            if (accommodationTypes == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<GetAccommodationTypeDto>>(accommodationTypes);
        }

        // create a new accommodationtype
        [HttpPost]
        public async Task<ActionResult<GetAccommodationTypeDto>> AddAccommodationType(CreateAccommodationTypeDto accommodationType)
        {
            AccommodationType accommodationTypeToAdd = _mapper.Map<AccommodationType>(accommodationType);
            _context.AccommodationTypes.Add(accommodationTypeToAdd);
            await _context.SaveChangesAsync();
            GetAccommodationTypeDto accommodationTypeToReturn = _mapper.Map<GetAccommodationTypeDto>(accommodationTypeToAdd);

            return CreatedAtAction(nameof(GetAccommodationType), new { id = accommodationTypeToReturn.Id }, accommodationTypeToReturn);
        }

        // update an existing accommodationtype
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccommodationType(int id, UpdateAccommodationTypeDto updateAccommodationTypeDto)
        {
            var accommodationType = await _context.AccommodationTypes.SingleAsync(a => a.Id == id);

            if (accommodationType == null)
            {
                return NotFound();
            }

            _mapper.Map(updateAccommodationTypeDto, accommodationType);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // delete an existing accommodationtype
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccommodationType(int id)
        {
            var accommodationType = await _context.AccommodationTypes.SingleAsync(a => a.Id == id);

            if (accommodationType == null)
            {
                return NotFound();
            }

            _context.AccommodationTypes.Remove(accommodationType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
