using Microsoft.EntityFrameworkCore;
using FitnessOperationsApi.Data;
using FitnessOperationsApi.Models;

namespace FitnessOperationsApi.Repositories.Members;

public class MemberRepository : IMemberRepository
{
    private readonly AppDbContext _context;

    public MemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Member> CreateAsync(Member member)
    {
        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();
        return member;
    }

    // Include = load related table data
    public async Task<List<Member>> GetAllAsync()
    {
        return await _context.Members.Include(m => m.HomeBranch).Where(m => !m.IsDeleted).ToListAsync();
    }

    // “When I fetch a Member, also fetch their Branch automatically.”
    public async Task<Member?> GetByIdAsync(Guid id)
    {
        return await _context.Members.Include(m => m.HomeBranch).FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
    }

    public async Task<Member?> GetByEmailAsync(string email)
    {
        return await _context.Members.FirstOrDefaultAsync(m => m.Email == email && !m.IsDeleted);
    }

    public async Task UpdateAsync(Member member)
    {
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
