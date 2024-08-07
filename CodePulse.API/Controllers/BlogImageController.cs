using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public BlogImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await imageRepository.GetAllImages();
            // Convert Domain model to DTO
            var response = new List<BlogImageDto>();
            foreach (var image in images)
            {
                response.Add(new BlogImageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url
                });
            }
            return Ok(response);
        }



        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,
                };
                blogImage = await imageRepository.Upload(file, blogImage);

                // cover domain model to dto
                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                };
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadImages([FromForm] IFormFileCollection files, [FromForm] string title)
        {
            if (ModelState.IsValid)
            {
                var blogImages = new List<BlogImage>();
                int count = 1;
                foreach (var file in files)
                {
                    if (!ModelState.IsValid)
                    {
                        continue;
                    }
                    ValidateFileUpload(file);
                    var blogImage = new BlogImage
                    {
                        FileExtension = Path.GetExtension(file.FileName).ToLower(),
                        FileName = title + $"{count}",
                        Title = title,
                        DateCreated = DateTime.Now,
                    };
                    count++;
                    await imageRepository.Upload(file, blogImage);
                    blogImages.Add(blogImage);
                }
                var response = new List<BlogImageDto>();
                foreach (var file in blogImages)
                {
                    var image = new BlogImageDto
                    {
                        Id = file.Id,
                        Title = file.Title,
                        DateCreated = file.DateCreated,
                        FileExtension = file.FileExtension,
                        FileName = file.FileName,
                        Url = file.Url
                    };
                    response.Add(image);
                }
                return Ok(response.ToList());
            }
            return BadRequest();
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowExtension = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowExtension.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }

        

    }
}