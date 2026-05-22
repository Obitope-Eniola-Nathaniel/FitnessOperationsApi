namespace FitnessOperationsApi.DTOs.Members;

public class MemberResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string MembershipType { get; set; } = default!;

    // Relationship data
    public string HomeBranchName { get; set; } = default!;
    public string HomeBranchLocation { get; set; } = default!;
}
