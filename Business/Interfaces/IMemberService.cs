using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IMemberService
    {
        Task<MemberResult> AddUserAsync(MemberSignUpForm form);
        Task<MemberResult> AddUserToRole(string memberId, string roleName);
        Task<MemberResult> CreateUserAsync(SignUpFormData formData, string roleName = "User");
        Task<IEnumerable<Member>> GetAllMembers();
        Task<MemberResult> GetMembersAsync();
    }
}