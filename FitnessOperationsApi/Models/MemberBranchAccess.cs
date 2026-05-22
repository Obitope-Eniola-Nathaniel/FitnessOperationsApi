namespace FitnessOperationsApi.Models;

public class MemberBranchAccess
{
    public Guid Id { get; set; }

    public Guid MemberId { get; set; }

    public Guid BranchId { get; set; }

    public DateTime AccessStartDate { get; set; }

    public DateTime? AccessEndDate { get; set; }

    public bool Status { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    /*
        Navigation Properties

        Helps EF Core move between objects
    */

    public Member Member { get; set; } = default!;

    public Branch Branch { get; set; } = default!;
}