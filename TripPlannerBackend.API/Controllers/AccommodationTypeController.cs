﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        // create a new accommodationtype
        [Authorize(Policy = "adminRights")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateAccommodationTypeDto createAccommodationTypeDto)
        {
            AccommodationType accommodationTypeToAdd = _mapper.Map<AccommodationType>(createAccommodationTypeDto);
            _context.AccommodationTypes.Add(accommodationTypeToAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { });
        }

        // get accomidation type by id
        [Authorize(Policy = "adminRights")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAccommodationTypeDto>> GetById([FromRoute] int id)
        {
            var accommodationType = await _context.AccommodationTypes.FindAsync(id);
            if (accommodationType == null) return NotFound();

            return _mapper.Map<GetAccommodationTypeDto>(accommodationType);
        }

        // get all accommodationtypes
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GetAccommodationTypeDto>>> GetAll()
        {
            var accommodationTypes = await _context.AccommodationTypes.Include(at => at.Accommodations).ToListAsync();
            if (!accommodationTypes.Any()) return NotFound();

            List<GetAccommodationTypeDto> getAccommodationTypeDtos = _mapper.Map<List<GetAccommodationTypeDto>>(accommodationTypes);
            for (int i = 0; i < getAccommodationTypeDtos.Count; i++)
            {
                if (accommodationTypes[i].Accommodations == null)
                {
                    getAccommodationTypeDtos[i].AccommodationCount = 0;
                }
                else
                {
                    getAccommodationTypeDtos[i].AccommodationCount = accommodationTypes[i].Accommodations!.Count;
                }
            }
            return getAccommodationTypeDtos;
        }

        // update an existing accommodationtype
        [Authorize(Policy = "adminRights")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAccommodationTypeDto updateAccommodationTypeDto)
        {
            var accommodationType = await _context.AccommodationTypes.FindAsync(id);
            if (accommodationType == null) return NotFound();

            _mapper.Map(updateAccommodationTypeDto, accommodationType);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // delete an existing accommodationtype
        [Authorize(Policy = "adminRights")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var accommodationType = await _context.AccommodationTypes.FindAsync(id);
            if (accommodationType == null) return NotFound();

            try
            {
                _context.AccommodationTypes.Remove(accommodationType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException _)
            {
                return StatusCode(409, "Delete operation failed. Accommodationtype is being referenced by accommodations.");
            }

            return NoContent();
        }

    }
}