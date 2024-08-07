using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<Blogpost> CreateAsync(Blogpost blogpost);

        Task<IEnumerable<Blogpost>> GetAllBlogPostsAsync();

        Task<Blogpost> GetByIdAsync(Guid id);

        Task<Blogpost?> UpdateAsync(Blogpost blogpost);

        Task<Blogpost?> DeleteAsync(Guid id);

        Task<Blogpost?> GetByUrlHandleAsync(string urlHandle);
    }
}
