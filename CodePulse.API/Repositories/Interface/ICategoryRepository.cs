using CodePulse.API.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAync(Category category);

        Task<IEnumerable<Category>> GetAllAsync(string? query,
                                    string? sortBy = null,
                                    string? sortDirection = null,
                                    int? pageNumber = 1,
                                    int? pageSize = 100);

        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> UpdateAsync(Category category);
        Task<Category?> DeleteAsync(Guid id);
    }
}
