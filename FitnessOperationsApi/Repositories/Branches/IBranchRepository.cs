
using FitnessOperationsApi.Models;

namespace FitnessOperationsApi.Repositories.Branches;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(Guid id);

    Task<Branch?> GetByEmailAsync(string email);

    Task<List<Branch>> GetAllAsync();

    Task<Branch> CreateAsync(Branch branch);

    Task UpdateAsync(Branch branch);

    Task SoftDeleteAsync(Branch branch);
}





 