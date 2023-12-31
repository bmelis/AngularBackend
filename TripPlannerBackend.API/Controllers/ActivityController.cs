﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.API.Services;
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
        private readonly TripAuthorizationService _tripAuthorizationService;
        public ActivityController(TripPlannerDbContext context, IMapper mapper, TripAuthorizationService tripAuthorizationService)
        {
            _context = context;
            _mapper = mapper;
            _tripAuthorizationService = tripAuthorizationService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GetActivityDto>> Create([FromBody] CreateActivityDto createActivityDto, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            DAL.Entity.Activity activityToAdd = _mapper.Map<DAL.Entity.Activity>(createActivityDto);
            await _context.Activities.AddAsync(activityToAdd);
            await _context.SaveChangesAsync();
            GetActivityDto activityToReturn = _mapper.Map<GetActivityDto>(activityToAdd);

            return CreatedAtAction(nameof(Create), new { id = activityToReturn.Id }, activityToReturn);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<GetActivityDto>> Update([FromRoute] int id, [FromBody] CreateActivityDto updateActivityDto, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            DAL.Entity.Activity? activity = await _context.Activities.FindAsync(id);
            if (activity == null) return NotFound();

            DAL.Entity.Activity updatedActivity = _mapper.Map(updateActivityDto, activity);
            await _context.SaveChangesAsync();
            GetActivityDto getActivityDto = _mapper.Map<GetActivityDto>(updatedActivity);

            return Ok(getActivityDto);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            DAL.Entity.Activity? activity = await _context.Activities.FindAsync(id);            
            if (activity == null) return NotFound();

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
