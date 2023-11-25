using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.DAL;
using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly TripPlannerDbContext _context;
        private readonly IMapper _mapper;
        public ActivityController(TripPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<GetActivityDto>> Create([FromBody] CreateActivityDto createActivityDto)
        {
            DAL.Entity.Activity activityToAdd = _mapper.Map<DAL.Entity.Activity>(createActivityDto);
            await _context.Activities.AddAsync(activityToAdd);
            await _context.SaveChangesAsync();
            GetActivityDto activityToReturn = _mapper.Map<GetActivityDto>(activityToAdd);

            return CreatedAtAction(nameof(Create), new { id = activityToReturn.Id }, activityToReturn);
        }

        //Get By ID
        [HttpGet("{destinationId}")]
        //[Authorize]
        //[Authorize(Policy = "ActivityReadAccess")]
        public async Task<ActionResult<List<GetActivityDto>>> GetByDestinationId(int destinationId)
        {
            List<DAL.Entity.Activity> activities = await _context.Activities.Where(a => a.DestinationId == destinationId).ToListAsync();
            if (!activities.Any()) return NotFound();

            return _mapper.Map<List<GetActivityDto>>(activities);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetActivityDto>> Update(int id, [FromBody] CreateActivityDto updateActivityDto)
        {
            DAL.Entity.Activity? activity = await _context.Activities.FindAsync(id);
            if (activity == null) return NotFound();

            DAL.Entity.Activity updatedActivity = _mapper.Map(updateActivityDto, activity);
            await _context.SaveChangesAsync();
            GetActivityDto getActivityDto = _mapper.Map<GetActivityDto>(updatedActivity);

            return Ok(getActivityDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            DAL.Entity.Activity? activity = await _context.Activities.FindAsync(id);            
            if (activity == null) return NotFound();

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
