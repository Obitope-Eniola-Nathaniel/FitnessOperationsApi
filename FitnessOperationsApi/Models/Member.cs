namespace FitnessOperationsApi.Models;

public class Member
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string MembershipType { get; set; } = default!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool Status { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    /*
        Foreign Key
    */

    public Guid HomeBranchId { get; set; }

    /*
        Navigation Property
    */

    // navigation properties:: this member can navigate to it branch
    public Branch HomeBranch { get; set; } = default!;

    public ICollection<MemberBranchAccess> MemberBranchAccesses { get; set; } = new List<MemberBranchAccess>();
}