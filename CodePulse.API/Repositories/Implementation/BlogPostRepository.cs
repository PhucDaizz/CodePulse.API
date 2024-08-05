using AutoMapper;
using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public BlogPostRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<Blogpost> CreateAsync(Blogpost blogpost)
        {
            await dbContext.Blogposts.AddAsync(blogpost);
            await dbContext.SaveChangesAsync();
            return blogpost;
        }
    }
}
