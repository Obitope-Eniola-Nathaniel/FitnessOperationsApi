using FitnessOperationsApi.Models;

namespace FitnessOperationsApi.Repositories.Members;

public interface IMemberRepository
{
    Task<Member> CreateAsync(Member member);

    Task<List<Member>> GetAllAsync();

    Task<Member?> GetByIdAsync(Guid id);

    Task<Member?> GetByEmailAsync(string email);

    Task UpdateAsync(Member member);

    Task SaveChangesAsync();
}
