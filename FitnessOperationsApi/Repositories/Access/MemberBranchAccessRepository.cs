using Dapper;
using FitnessOperationsApi.Data;
using FitnessOperationsApi.DTOs.Access;
using FitnessOperationsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessOperationsApi.Repositories.Access;

public class MemberBranchAccessRepository : IMemberBranchAccessRepository
{
    private readonly AppDbContext _context;
    private readonly DapperContext _dapperContext;

    public MemberBranchAccessRepository(AppDbContext context, DapperContext dapperContext)
    {
        _context = context;
        _dapperContext = dapperContext;
    }

    // Duplicate Assignment Check
    // What business rule must be enforced?
    public async Task<bool> AssignmentExistsAsync(Guid memberId, Guid branchId)
    {
        return await _context.MemberBranchAccesses.AnyAsync(x => x.MemberId == memberId && x.BranchId == branchId && x.Status);
    }

    // Create Assignment Method
    public async Task<MemberBranchAccess> AssignAsync(MemberBranchAccess access)
    {
        await _context.MemberBranchAccesses.AddAsync(access);

        await _context.SaveChangesAsync();

        return access;
    }

    // Get Branches Assigned To Member
    public async Task<IEnumerable<MemberBranchAccessResponse>>GetBranchesForMemberAsync(Guid memberId)
    {
        var query = @"
                    SELECT
                        mba.""MemberId"",
                        CONCAT(m.""FirstName"", ' ', m.""LastName"")
                            AS ""MemberName"",
                        b.""Id"" AS ""BranchId"",
                        b.""Name"" AS ""BranchName"",
                        mba.""AccessStartDate"",
                        mba.""AccessEndDate""
                    FROM ""MemberBranchAccesses"" mba
                    INNER JOIN ""Members"" m
                        ON mba.""MemberId"" = m.""Id""
                    INNER JOIN ""Branches"" b
                        ON mba.""BranchId"" = b.""Id""
                    WHERE mba.""MemberId"" = @MemberId
                    AND mba.""Status"" = TRUE
                ";

        using var connection = _dapperContext.CreateConnection();

        var result = await connection.QueryAsync<MemberBranchAccessResponse>(query, new { MemberId = memberId });

        return result;
    }


    public async Task<IEnumerable<MemberBranchAccessResponse>>GetMembersInBranchAsync(Guid branchId)
    {
        var query = @"
                    SELECT
                        m.""Id"" AS ""MemberId"",
                        CONCAT(m.""FirstName"", ' ', m.""LastName"")
                            AS ""MemberName"",
                        b.""Id"" AS ""BranchId"",
                        b.""Name"" AS ""BranchName"",
                        mba.""AccessStartDate"",
                        mba.""AccessEndDate""
                    FROM ""MemberBranchAccesses"" mba
                    INNER JOIN ""Members"" m
                        ON mba.""MemberId"" = m.""Id""
                    INNER JOIN ""Branches"" b
                        ON mba.""BranchId"" = b.""Id""
                    WHERE mba.""BranchId"" = @BranchId
                    AND mba.""Status"" = TRUE
                ";

        using var connection = _dapperContext.CreateConnection();

        var result = await connection.QueryAsync<MemberBranchAccessResponse>(query, new { BranchId = branchId });

        return result;
    }

    // Revoke Access
    public async Task<bool> RevokeAccessAsync(Guid memberId, Guid branchId)
    {
        var access = await _context.MemberBranchAccesses.FirstOrDefaultAsync(x => x.MemberId == memberId && x.BranchId == branchId && x.Status);

        if (access is null)
        {
            return false;
        }

        access.Status = false;

        await _context.SaveChangesAsync();

        return true;
    }
}