using FitnessOperationsApi.Common;
using FitnessOperationsApi.DTOs.Access;
using FitnessOperationsApi.Models;
using FitnessOperationsApi.Repositories.Access;
using Microsoft.AspNetCore.Mvc;

namespace FitnessOperationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberAccessController : ControllerBase
{
    private readonly IMemberBranchAccessRepository _repository;

    public MemberAccessController(IMemberBranchAccessRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign(AssignMemberBranchRequest request)
    {
        var exists =await _repository.AssignmentExistsAsync(request.MemberId, request.BranchId);

        if (exists)
        {
            return Conflict(new ApiResponse<object>
            {
                Message = "Member already assigned",
                ResponseCode = "99"
            });
        }

        var access = new MemberBranchAccess
        {
            Id = Guid.NewGuid(),
            MemberId = request.MemberId,
            BranchId = request.BranchId,
            AccessStartDate = request.AccessStartDate,
            AccessEndDate = request.AccessEndDate,
            Status = true
        };

        await _repository.AssignAsync(access);

        return Ok(new ApiResponse<object>
        {
            Message = "Access assigned successfully",
            ResponseCode = "00"
        });
    }


    [HttpGet("member/{memberId}")]
    public async Task<IActionResult> GetBranchesForMember(Guid memberId)
    {
        var result = await _repository.GetBranchesForMemberAsync(memberId);

        return Ok(new ApiResponse<object>
        {
            Message = "Successful",
            ResponseCode = "00",
            Data = result
        });
    }

    [HttpDelete("revoke")]
    public async Task<IActionResult> Revoke(Guid memberId, Guid branchId)
    {
        var revoked =await _repository.RevokeAccessAsync(memberId, branchId);

        if (!revoked)
        {
            return NotFound(new ApiResponse<object>
            {
                Message = "Access record not found",
                ResponseCode = "99"
            });
        }

        return Ok(new ApiResponse<object>
        {
            Message = "Access revoked successfully",
            ResponseCode = "00"
        });
    }