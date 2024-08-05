using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<Blogpost> CreateAsync(Blogpost blogpost);

    }
}
