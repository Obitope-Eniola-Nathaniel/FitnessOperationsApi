namespace FitnessOperationsApi.DTOs.Access;

public class MemberBranchAccessResponse
{
    public Guid MemberId { get; set; }

    public string MemberName { get; set; } = default!;

    public Guid BranchId { get; set; }

    public string BranchName { get; set; } = default!;

    public DateTime AccessStartDate { get; set; }

    public DateTime? AccessEndDate { get; set; }
}
