
using Microsoft.AspNetCore.Mvc;
using FitnessOperationsApi.Common;
using FitnessOperationsApi.DTOs.Branches;
using FitnessOperationsApi.Models;
using FitnessOperationsApi.Repositories.Branches;

namespace FitnessOperationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BranchesController : Controller
{
    private readonly IBranchRepository _branchRepository;

    public BranchesController(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }


    [HttpPost]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Message = "Validation failed",
                ResponseCode = "99",
                Data = ModelState
            });
        }

        var existingBranch = await _branchRepository.GetByEmailAsync(request.Email);

        if (existingBranch is not null)
        {
            return Conflict(new ApiResponse<object>
            {
                Message = "Branch email already exists",
                ResponseCode = "99"
            });
        }

        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Location = request.Location,
            ManagerName = request.ManagerName,
            OpeningTime = request.OpeningTime,
            ClosingTime = request.ClosingTime
        };

        var createdBranch = await _branchRepository.CreateAsync(branch);

        var response = MapToResponse(createdBranch);

        return Ok(new ApiResponse<BranchResponse>
        {
            Message = "Branch created successfully",
            ResponseCode = "00",
            Data = response
        });

    }

    [HttpGet]
    public async Task<IActionResult> GetAllBranches()
    {
        var branches = await _branchRepository.GetAllAsync();

        var response = branches.Select(MapToResponse).ToList();

        return Ok(new ApiResponse<List<BranchResponse>>
        {
            Message = "Branches retrieved successfully",
            ResponseCode = "00",
            Data = response
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBranchById(Guid id)
    {
        var branch = await _branchRepository.GetByIdAsync(id);

        if (branch is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Message = "Branch not found",
                ResponseCode = "99"
            });
        }

        return Ok(new ApiResponse<BranchResponse>
        {
            Message = "Branch retrieved successfully",
            ResponseCode = "00",
            Data = MapToResponse(branch)
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBranch( Guid id, [FromBody] UpdateBranchRequest request)
    {
        var branch = await _branchRepository.GetByIdAsync(id);

        if (branch is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Message = "Branch not found",
                ResponseCode = "99"
            });
        }

        branch.Name = request.Name;
        branch.Email = request.Email;
        branch.PhoneNumber = request.PhoneNumber;
        branch.Location = request.Location;
        branch.ManagerName = request.ManagerName;
        branch.OpeningTime = request.OpeningTime;
        branch.ClosingTime = request.ClosingTime;

        await _branchRepository.UpdateAsync(branch);

        return Ok(new ApiResponse<BranchResponse>
        {
            Message = "Branch updated successfully",
            ResponseCode = "00",
            Data = MapToResponse(branch)
        });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBranch(Guid id)
    {
        var branch = await _branchRepository.GetByIdAsync(id);

        if (branch is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Message = "Branch not found",
                ResponseCode = "99"
            });
        }

        await _branchRepository.SoftDeleteAsync(branch);

        return Ok(new ApiResponse<object>
        {
            Message = "Branch deleted successfully",
            ResponseCode = "00"
        });
    }

    private static BranchResponse MapToResponse(Branch branch)
    {
        return new BranchResponse
        {
            Id = branch.Id,
            Name = branch.Name,
            Email = branch.Email,
            PhoneNumber = branch.PhoneNumber,
            Location = branch.Location,
            ManagerName = branch.ManagerName,
            OpeningTime = branch.OpeningTime,
            ClosingTime = branch.ClosingTime
        };
    }
}

