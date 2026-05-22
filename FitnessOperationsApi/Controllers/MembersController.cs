using FitnessOperationsApi.Common;
using FitnessOperationsApi.DTOs.Members;
using FitnessOperationsApi.Models;
using FitnessOperationsApi.Repositories.Members;
using Microsoft.AspNetCore.Mvc;

namespace FitnessOperationsApi.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberRepository _memberRepository;

    public MembersController(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberRequest request)
    {
        var existing = await _memberRepository.GetByEmailAsync(request.Email);

        if (existing != null)
        {
            return Conflict(new ApiResponse<object>
            {
                Message = "Email already exists",
                ResponseCode = "99"
            });
        }

        var member = new Member
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            MembershipType = request.MembershipType,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            HomeBranchId = request.HomeBranchId
        };

        var created = await _memberRepository.CreateAsync(member);

        return Ok(new ApiResponse<object>
        {
            Message = "Member created successfully",
            ResponseCode = "00",
            Data = created
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var members = await _memberRepository.GetAllAsync();

        var result = members.Select(m => new MemberResponse
        {
            Id = m.Id,
            FullName = $"{m.FirstName} {m.LastName}",
            Email = m.Email,
            PhoneNumber = m.PhoneNumber,
            MembershipType = m.MembershipType,
            HomeBranchName = m.HomeBranch.Name,
            HomeBranchLocation = m.HomeBranch.Location
        });

        return Ok(new ApiResponse<object>
        {
            Message = "Members retrieved successfully",
            ResponseCode = "00",
            Data = result
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var member = await _memberRepository.GetByIdAsync(id);

        if (member == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Message = "Member not found",
                ResponseCode = "404"
            });
        }

        var result = new MemberResponse
        {
            Id = member.Id,
            FullName = $"{member.FirstName} {member.LastName}",
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            MembershipType = member.MembershipType,
            HomeBranchName = member.HomeBranch.Name,
            HomeBranchLocation = member.HomeBranch.Location
        };

        return Ok(new ApiResponse<MemberResponse>
        {
            Message = "Member retrieved successfully",
            ResponseCode = "00",
            Data = result
        });
    }
}