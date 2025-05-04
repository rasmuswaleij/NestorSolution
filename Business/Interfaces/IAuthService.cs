using Business.Models;
using Domain.Dtos;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(MemberLoginForm loginForm);
    Task LogoutAsync();
    Task<bool> SignUpAsync(MemberSignUpForm form);
    Task<AuthResult> SignUpAsync2(SignUpFormData formData);
    Task<AuthResult> SignInAsync(SignInFormData formData);
}
