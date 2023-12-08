using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using TripPlannerBackend.API.Dto;
using TripPlannerBackend.API.Services;
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
        private readonly TripAuthorizationService _tripAuthorizationService;
        private readonly PublicImageManagementService _imageManagementService;
        private readonly string _serverUrl;
        public DestinationController(TripPlannerDbContext context, IMapper mapper, TripAuthorizationService tripAuthorizationService, PublicImageManagementService imageManagementService)
        {
            _context = context;
            _mapper = mapper;
            _tripAuthorizationService = tripAuthorizationService;
            _imageManagementService = imageManagementService;
            _serverUrl = Environment.GetEnvironmentVariable("SERVER_URL");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GetDestinationDto>> Create([FromBody] CreateDestinationDto createDestinationDto)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(createDestinationDto.TripId, email)) return StatusCode(403);

            Destination destinationToAdd = _mapper.Map<Destination>(createDestinationDto);

            // Set image to random placeholder
            Random random = new Random();
            int idx = random.Next(1, 3);
            destinationToAdd.ImageUrl = $"{_serverUrl}/images/placeholders/{idx}.webp";

            await _context.Destinations.AddAsync(destinationToAdd);
            await _context.SaveChangesAsync();
            GetDestinationDto destinationToReturn = _mapper.Map<GetDestinationDto>(destinationToAdd);

            return CreatedAtAction(nameof(Create), new { id = destinationToReturn.Id }, destinationToReturn);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<GetDestinationDto>> Update([FromRoute] int id, [FromBody] CreateDestinationDto updateDestinationDto)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(updateDestinationDto.TripId, email)) return StatusCode(403);

            Destination? destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            Destination updatedDestination = _mapper.Map(updateDestinationDto, destination);
            await _context.SaveChangesAsync();
            GetDestinationDto getDestinationDto = _mapper.Map<GetDestinationDto>(updatedDestination);

            return Ok(getDestinationDto);
        }

        [Authorize]
        [HttpPatch("{id}/image/upload")]
        public async Task<ActionResult<GetDestinationDto>> UpdateImageViaUpload([FromRoute] int id, [FromForm] IFormFile image)
        {
            List<string> extensions = new List<string> { "jpg", "jpeg", "png" };
            if (!_imageManagementService.HasCorrectExtension(image, extensions))
            {
                return StatusCode(415, "Unsupported file extension");
            }

            long maxFileSize = 1024 * 1024; // 1MB
            if (_imageManagementService.IsLargerThan(image, maxFileSize))
            {
                return StatusCode(413, "Image is too large");
            }

            Destination? destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(destination.TripId, email)) return StatusCode(403);

            // Deletes if is not default image
            _imageManagementService.Delete(destination.ImageUrl);

            string fileName = await _imageManagementService.Create(image);

            string url = $"{_serverUrl}/images/{fileName}";
            destination.ImageUrl = url;
            await _context.SaveChangesAsync();

            GetDestinationDto getDestinationDto = _mapper.Map<GetDestinationDto>(destination);            
            return Ok(getDestinationDto);
        }

        [Authorize]
        [HttpPatch("{id}/image/default")]
        public async Task<ActionResult<GetDestinationDto>> UpdateImageViaDefault([FromRoute] int id, [FromBody] int imageId)
        {            
            string fileName;
            try
            {
                fileName = _imageManagementService.GetDefaultImagePath(imageId);
            } catch (FileNotFoundException _)
            {
                return BadRequest($"No image with this id exists. ID: {imageId}");
            }

            Destination? destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(destination.TripId, email)) return StatusCode(403);

            _imageManagementService.Delete(destination.ImageUrl);

            destination.ImageUrl = $"{_serverUrl}/images/placeholders/{fileName}";
            await _context.SaveChangesAsync();

            GetDestinationDto getDestinationDto = _mapper.Map<GetDestinationDto>(destination);
            return Ok(getDestinationDto);
        }

        [Authorize]
        [HttpPatch("{id}/image/unsplash")]
        public async Task<ActionResult<GetDestinationDto>> UpdateImageViaUnsplash([FromRoute] int id, [FromBody] UnsplashRequest unsplashRequest)
        {
            if (!unsplashRequest.Url.StartsWith("https://images.unsplash.com/photo-"))
            {
                return BadRequest("Please use an unsplash link");
            }

            Destination? destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(destination.TripId, email)) return StatusCode(403);

            if (!destination.ImageUrl.StartsWith("https://images.unsplash.com/photo-"))
            {
                _imageManagementService.Delete(destination.ImageUrl);
            }

            destination.ImageUrl = unsplashRequest.Url;
            await _context.SaveChangesAsync();

            GetDestinationDto getDestinationDto = _mapper.Map<GetDestinationDto>(destination);
            return Ok(getDestinationDto);
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id, [FromQuery] int tripId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (await _tripAuthorizationService.IsParticipantOrNull(tripId, email)) return StatusCode(403);

            Destination? destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
