using AutoMapper;
using CodePulse.API.Data;
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
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ApplicationDbContext dbContext, IMapper mapper, ICategoryRepository categoryRepository)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }


        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto requestDto)
        {
            var category = mapper.Map<Category>(requestDto);

            await categoryRepository.CreateAync(category);

            var response = mapper.Map<CategoryDto>(category);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategory([FromQuery] string? query,
                                    [FromQuery] string? sortBy,
                                    [FromQuery] string? sortDirection,
                                    [FromQuery] int? pageNumber,
                                    [FromQuery] int? pageSize)
        {
            var caterogies = await categoryRepository.GetAllAsync(query, sortBy, sortDirection, pageNumber, pageSize);

            //var response = new List<CategoryDto>();

            var response = mapper.Map<List<CategoryDto>>(caterogies);

            //foreach (var category in caterogies)
            //{
            //    response.Add(new CategoryDto
            //    {
            //        Id = category.Id,
            //        Name = category.Name,
            //        UrlHandle = category.UrlHandle,
            //    });
            //}
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var existingCategory = await categoryRepository.GetByIdAsync(id);

            if(existingCategory == null)
            {
                return NotFound();
            }

            var response = mapper.Map<CategoryDto> (existingCategory);
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory([FromRoute]Guid id, UpdateCategoryRequestDto request)
        {
            var category = mapper.Map<Category>(request);
            category.Id = id;

            category = await categoryRepository.UpdateAsync(category);

            if(category == null)
            {
                return NotFound();
            }

            var response = mapper.Map<CategoryDto>(category);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute]Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = mapper.Map<Category>(category);

            return Ok(response);
        }

    }
}
