namespace FitnessOperationsApi.DTOs.Branches;

public class BranchResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string Location { get; set; } = default!;

    public string ManagerName { get; set; } = default!;

    public TimeSpan OpeningTime { get; set; }

    public TimeSpan ClosingTime { get; set; }
}