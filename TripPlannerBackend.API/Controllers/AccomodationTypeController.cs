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
    public class AccomodationTypeController : ControllerBase
    {
        private readonly TripPlannerDbContext _context;
        private readonly IMapper _mapper;

        public AccomodationTypeController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // get accomidation type by id
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAccomodationTypeDto>> GetAccomodationType(int id)
        {
            var accomodationType = await _context.AccomodationTypes.SingleAsync(a => a.Id == id);

            if (accomodationType == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetAccomodationTypeDto>(accomodationType);
        }

        // get all accomodationtypes
        [HttpGet]
        public async Task<ActionResult<List<GetAccomodationTypeDto>>> GetAccomodationTypes()
        {
            var accomodationTypes = await _context.AccomodationTypes.ToListAsync();

            if (accomodationTypes == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<GetAccomodationTypeDto>>(accomodationTypes);
        }

        // create a new accomodationtype
        [HttpPost]
        public async Task<ActionResult<GetAccomodationTypeDto>> AddAccomodationType(CreateAccomodationTypeDto accomodationType)
        {
            AccomodationType accomodationTypeToAdd = _mapper.Map<AccomodationType>(accomodationType);
            _context.AccomodationTypes.Add(accomodationTypeToAdd);
            await _context.SaveChangesAsync();
            GetAccomodationTypeDto accomodationTypeToReturn = _mapper.Map<GetAccomodationTypeDto>(accomodationTypeToAdd);

            return CreatedAtAction(nameof(GetAccomodationType), new { id = accomodationTypeToReturn.Id }, accomodationTypeToReturn);
        }

        // update an existing accomodationtype
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccomodationType(int id, UpdateAccomodationTypeDto updateAccomodationTypeDto)
        {
            var accomodationType = await _context.AccomodationTypes.SingleAsync(a => a.Id == id);

            if (accomodationType == null)
            {
                return NotFound();
            }

            _mapper.Map(updateAccomodationTypeDto, accomodationType);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // delete an existing accomodationtype
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccomodationType(int id)
        {
            var accomodationType = await _context.AccomodationTypes.SingleAsync(a => a.Id == id);

            if (accomodationType == null)
            {
                return NotFound();
            }

            _context.AccomodationTypes.Remove(accomodationType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
