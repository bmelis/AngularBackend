using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.DAL;

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

        //Get By ID
        [HttpGet("{id}")]
        //[Authorize]
        //[Authorize(Policy = "ActivityReadAccess")]
        public async Task<ActionResult<List<GetActivityDto>>> GetActivity(int id)
        {
            var activity = await _context.Activities.Where(a => a.TripId == id).ToListAsync();

            if (activity == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<GetActivityDto>>(activity);
        }
    }
}
