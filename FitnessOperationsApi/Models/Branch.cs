namespace FitnessOperationsApi.Models;

public class Branch
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string Location { get; set; } = default!;

    public string ManagerName { get; set; } = default!;

    public TimeSpan OpeningTime { get; set; }

    public TimeSpan ClosingTime { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    /*
        Navigation Properties
    */
    // A branch contains many members
    public ICollection<Member> Members { get; set; } = new List<Member>();

    public ICollection<MemberBranchAccess> MemberBranchAccesses { get; set; } = new List<MemberBranchAccess>();
}