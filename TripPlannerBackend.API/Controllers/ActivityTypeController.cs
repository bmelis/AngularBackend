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
    public class ActivityTypeController : ControllerBase
    {
        private readonly TripPlannerDbContext _context;
        private readonly IMapper _mapper;
        public ActivityTypeController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Get By ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GetActivityTypeDto>> GetActivityType(int id)
        {
            var activityType = await _context.ActivityTypes.Include(t => t.Activities).SingleAsync(t => t.Id == id);

            if (activityType == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetActivityTypeDto>(activityType);
        }

        // Get ALL
        [HttpGet]
        public async Task<ActionResult<List<GetActivityTypeDto>>> GetActivityTypes()
        {
            var activityTypes = await _context.ActivityTypes.Include(t => t.Activities).ToListAsync();

            if (activityTypes == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<GetActivityTypeDto>>(activityTypes);
        }

        //Insert - you have to be authenticated
        [HttpPost]
        public async Task<ActionResult<GetActivityTypeDto>> AddActivityType(CreateActivityTypeDto activiyType)
        {
            //We map the CreateTripDto to the Trip entity object
            ActivityType activityTypeToAdd = _mapper.Map<ActivityType>(activiyType);
            _context.ActivityTypes.Add(activityTypeToAdd);
            await _context.SaveChangesAsync();
            GetActivityTypeDto activityTypeToReturn = _mapper.Map<GetActivityTypeDto>(activityTypeToAdd);

            return CreatedAtAction(nameof(AddActivityType), new { id = activityTypeToReturn.Id }, activityTypeToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetActivityTypeDto>> UpdateActivityType(int id, UpdateActivityTypeDto updateActivityType)
        {
            var existingActivityType = await _context.ActivityTypes.FindAsync(id);

            _mapper.Map(updateActivityType, existingActivityType);
            await _context.SaveChangesAsync();

            GetActivityTypeDto updatedActivityTypeDto = _mapper.Map<GetActivityTypeDto>(existingActivityType);

            return Ok(updatedActivityTypeDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GetActivityTypeDto>> DeleteActivityType(int id)
        {
            var existingActivityType = await _context.ActivityTypes.FindAsync(id);

            if (existingActivityType == null)
            {
                return NotFound("ActivityType not found");
            }

            _context.ActivityTypes.Remove(existingActivityType);

            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
