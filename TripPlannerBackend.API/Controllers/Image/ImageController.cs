using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripPlannerBackend.API.Services;

namespace TripPlannerBackend.API.Controllers.Image
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly PublicImageManagementService _imageManagementService;

        public ImageController(PublicImageManagementService imageManagementService) 
        {
            _imageManagementService = imageManagementService;
        }

        [Authorize]
        [HttpGet("previews")]
        public List<string> getAllDefaultPreviews()
        {
            return _imageManagementService.GetAllDefaultImagePreviews();
        }
    }
}
