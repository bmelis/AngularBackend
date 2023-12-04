using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.DAL;
using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityTypeController : ControllerBase
    {
        private readonly TripPlannerDbContext _context;
        private readonly IMapper _mapper;
        public ActivityTypeController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [Authorize(Policy = "adminRights")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateActivityTypeDto createActivityTypeDto)
        {
            //We map the CreateTripDto to the Trip entity object
            ActivityType activityTypeToAdd = _mapper.Map<ActivityType>(createActivityTypeDto);
            _context.ActivityTypes.Add(activityTypeToAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { });
        }
        [Authorize(Policy = "adminRights")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetActivityTypeDto>> GetById(int id)
        {
            var activityType = await _context.ActivityTypes.FindAsync(id);
            if (activityType == null) return NotFound();

            return _mapper.Map<GetActivityTypeDto>(activityType);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GetActivityTypeDto>>> GetAll()
        {
            var activityTypes = await _context.ActivityTypes.ToListAsync();
            if (!activityTypes.Any()) return NotFound();

            return _mapper.Map<List<GetActivityTypeDto>>(activityTypes);
        }

        [Authorize(Policy = "adminRights")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateActivityTypeDto updateActivityType)
        {
            var existingActivityType = await _context.ActivityTypes.FindAsync(id);
            if (existingActivityType == null) return NotFound();

            _mapper.Map(updateActivityType, existingActivityType);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Policy = "adminRights")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<GetActivityTypeDto>> Delete([FromRoute] int id)
        {
            var existingActivityType = await _context.ActivityTypes.FindAsync(id);
            if (existingActivityType == null) return NotFound();

            _context.ActivityTypes.Remove(existingActivityType);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
