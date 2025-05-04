namespace Domain.Models;

public class Project
{
    public string Id { get; set; } = null!;
    public string? Image { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public string ClientId { get; set; } = null!;
    public Client? Client { get; set; }

    public string MemberId { get; set; } = null!;
    public Member? Member { get; set; }

    public int StatusId { get; set; }
    public Status? Status { get; set; }

}
