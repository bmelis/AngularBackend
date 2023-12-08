using static System.Net.Mime.MediaTypeNames;
using TripPlannerBackend.DAL.Entity;

namespace TripPlannerBackend.API.Services
{
    public class PublicImageManagementService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PublicImageManagementService(IWebHostEnvironment env)
        {
            _webHostEnvironment = env;
        }

        public string GetDefaultImagePath(int imageId)
        {
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, $"images/placeholders/{imageId}.webp");

            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException();
            }

            return $"{imageId}.webp";
        }

        public List<string> GetAllDefaultImagePreviews()
        {
            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/placeholders/previews");            
            List<string> imagePaths = Directory.GetFiles(folderPath).ToList();

            List<string> imageUrls = new List<string>();
            foreach (string imageUrl in imagePaths)
            {
                string file = Path.GetFileName(imageUrl);
                string url = $"https://your-api-bmelis.cloud.okteto.net/images/placeholders/previews/{file}";
                imageUrls.Add(url);
            }

            return imageUrls;
        }

        public bool HasCorrectExtension(IFormFile image, List<string> extensions)
        {
            return extensions.Any(extension => image.FileName.ToLower().EndsWith($".{extension}"));
        }

        public bool IsLargerThan(IFormFile image, long maxFileSize)
        {
            if (image.Length > maxFileSize)
            {
                return true;
            }
            return false;
        }

        public void Delete(string url)
        {
            string? oldFileName = Path.GetFileName(new Uri(url).LocalPath);
            if (oldFileName == null || (oldFileName != "1.jpg" && oldFileName != "2.jpg"))
            {
                string oldFilePath = Path.Combine("wwwroot/images", oldFileName);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }
        }

        public async Task<string> Create(IFormFile image)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine("wwwroot/images", fileName);
            using (var stream = File.Create(filePath))
            {
                await image.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
