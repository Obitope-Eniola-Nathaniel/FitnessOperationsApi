using Microsoft.EntityFrameworkCore;
using FitnessOperationsApi.Data;
using FitnessOperationsApi.Models;


namespace FitnessOperationsApi.Repositories.Branches;

public class BranchRepository : IBranchRepository
{
    private readonly AppDbContext _context;

    public BranchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Branch?> GetByIdAsync(Guid id)
    {
        return await _context.Branches.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Branch?> GetByEmailAsync(string email)
    {
        return await _context.Branches.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && !x.IsDeleted);
    }

    public async Task<List<Branch>> GetAllAsync()
    {
        return await _context.Branches.Where(x => !x.IsDeleted).OrderByDescending(x => x.DateCreated).ToListAsync();
    }

    public async Task<Branch> CreateAsync(Branch branch)
    {
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();
        return branch;
    }

    public async Task UpdateAsync(Branch branch)
    {
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Branch branch)
    {
        branch.IsDeleted = true;
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
    }
}
