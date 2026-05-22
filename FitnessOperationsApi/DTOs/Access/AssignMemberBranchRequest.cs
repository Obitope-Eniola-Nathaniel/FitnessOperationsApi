namespace FitnessOperationsApi.DTOs.Access;

public class AssignMemberBranchRequest
{
    public Guid MemberId { get; set; }

    public Guid BranchId { get; set; }

    public DateTime AccessStartDate { get; set; }

    public DateTime? AccessEndDate { get; set; }
}
