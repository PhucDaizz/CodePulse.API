using AutoMapper;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;

namespace CodePulse.API.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateCategoryRequestDto,Category>().ReverseMap();

            CreateMap<Category,CategoryDto>().ReverseMap(); 

            CreateMap<UpdateCategoryRequestDto,Category>().ReverseMap();

            CreateMap<CreateBlogPostRequestDto, Blogpost>().
                ForMember(dest => dest.Categories, opt => opt.Ignore());

            CreateMap<Blogpost, BlogpostDto>().
                ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                })));

        }
    }
}
