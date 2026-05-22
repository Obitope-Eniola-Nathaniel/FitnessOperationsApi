using System.ComponentModel.DataAnnotations;

namespace FitnessOperationsApi.DTOs.Branches;

public class UpdateBranchRequest
{
    [Required]
    public string Name { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string PhoneNumber { get; set; } = default!;

    [Required]
    public string Location { get; set; } = default!;

    [Required]
    public string ManagerName { get; set; } = default!;

    public TimeSpan OpeningTime { get; set; }

    public TimeSpan ClosingTime { get; set; }

}
