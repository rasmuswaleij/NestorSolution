namespace Domain.Models;
public class Member
{
    public string Id { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone {  get; set; }
    public MemberAdress MemberAdress { get; set; } = null!;

}
