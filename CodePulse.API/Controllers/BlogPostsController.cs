using AutoMapper;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateBlogPost([FromBody]CreateBlogPostRequestDto request)
        {
            //  Map Dto to domain
            var blogpost = mapper.Map<Blogpost>(request);
            blogpost.Categories = new List<Category>();

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetByIdAsync(categoryGuid);
                if (existingCategory != null)
                {
                    blogpost.Categories.Add(existingCategory);
                }
            }

            blogpost = await blogPostRepository.CreateAsync(blogpost);

            // Map Domain model to Dto
            var reponse = mapper.Map<BlogpostDto>(blogpost);

            return Ok(reponse);

        }


        [HttpGet]   
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogposts = await blogPostRepository.GetAllBlogPostsAsync();

            var reponse = mapper.Map<List<BlogpostDto>>(blogposts);

            return Ok(reponse);
        }   

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute]Guid id)
        {
            var blogPost = await blogPostRepository.GetByIdAsync(id);
            if (blogPost != null)
            {
                var response = mapper.Map<BlogpostDto>(blogPost);
                return Ok(response); 
            }
            return NotFound();

        }


        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditBlogPost([FromRoute]Guid id, UpdateBlogPostRequestDto request)
        {
            
            var blogpost = mapper.Map<Blogpost>(request);
            blogpost.Id = id;
            foreach (var categoryID in request.Categories)
            {
                var existingCategory = await categoryRepository.GetByIdAsync(categoryID);
                if (existingCategory != null)
                {
                    blogpost.Categories.Add(existingCategory);
                }
            }
            var updateBlogPost = await blogPostRepository.UpdateAsync(blogpost);
            if(updateBlogPost != null)
            {
                var reponse = mapper.Map<BlogpostDto>(updateBlogPost);
                return Ok(reponse);
            }
            return NotFound();

        }


        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute]Guid id)
        {
            var deleteBlogPost = await blogPostRepository.DeleteAsync(id);

            if (deleteBlogPost != null)
            {
                var response = mapper.Map<BlogpostDto>(deleteBlogPost);
                return Ok(response);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blospost = await blogPostRepository.GetByUrlHandleAsync(urlHandle); 

            if (blospost != null)
            {
                var response = mapper.Map<BlogpostDto>(blospost);
                return Ok(response);
            }
            return NotFound();
        }
    }
}
