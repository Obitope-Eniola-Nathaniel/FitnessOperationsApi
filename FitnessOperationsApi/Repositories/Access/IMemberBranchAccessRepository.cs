using FitnessOperationsApi.DTOs.Access;
using FitnessOperationsApi.Models;

namespace FitnessOperationsApi.Repositories.Access;

public interface IMemberBranchAccessRepository
{
    Task<bool> AssignmentExistsAsync(Guid memberId, Guid branchId);

    Task<MemberBranchAccess> AssignAsync(MemberBranchAccess access);

    // Why Use DTOs for Dapper Queries?
    // Dapper maps SQL results directly into objects. Instead of returning entities:
    Task<IEnumerable<MemberBranchAccessResponse>>GetBranchesForMemberAsync(Guid memberId);

    Task<IEnumerable<MemberBranchAccessResponse>>GetMembersInBranchAsync(Guid branchId);

    Task<bool> RevokeAccessAsync(Guid memberId, Guid branchId);
}
