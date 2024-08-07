using AutoMapper;
using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Blogpost?> DeleteAsync(Guid id)
        {
            var existing = await dbContext.Blogposts.FirstOrDefaultAsync(x => x.Id == id);
            if (existing != null)
            {
                dbContext.Blogposts.Remove(existing);
                await dbContext.SaveChangesAsync();
                return existing;
            }
            return null;
        }

        public async Task<IEnumerable<Blogpost>> GetAllBlogPostsAsync()
        {
            return await dbContext.Blogposts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<Blogpost> GetByIdAsync(Guid id)
        {
            return await dbContext.Blogposts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Blogpost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await dbContext.Blogposts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<Blogpost?> UpdateAsync(Blogpost blogpost)
        {
            var existingBlogPost = await dbContext.Blogposts.Include(x => x.Categories).
                FirstOrDefaultAsync(x => x.Id == blogpost.Id);

            if (existingBlogPost != null)
            {
                dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogpost);

                existingBlogPost.Categories = blogpost.Categories;

                await dbContext.SaveChangesAsync();

                return blogpost;
            }
            return null;

        }
    }
}
