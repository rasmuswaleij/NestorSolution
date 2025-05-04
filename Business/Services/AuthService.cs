using Azure.Core;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService(SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, IMemberService memberService) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IMemberService _memberService = memberService;

    public async Task<bool> LoginAsync(MemberLoginForm loginForm)
    {
        var result = await _signInManager.PasswordSignInAsync(loginForm.Email, loginForm.Password, false, false);
        return result.Succeeded;

    }


    public async Task<AuthResult> SignInAsync(SignInFormData formData)
    {
        if (formData == null)
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "All required fields are not supplied." };


        var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, formData.IsPersistent, false);
        return result.Succeeded
            ? new AuthResult { Succeeded = true, StatusCode = 200 }
            : new AuthResult { Succeeded = false, StatusCode = 401, Error = "Invalid email or password"};
    }


    public async Task<AuthResult> SignUpAsync2(SignUpFormData formData)
    {
        if (formData == null)
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "All required fields are not supplied."};
        var result = await _memberService.CreateUserAsync(formData);
        return result.Succeeded
            ? new AuthResult { Succeeded = true, StatusCode = 201 }
            : new AuthResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<bool> SignUpAsync(MemberSignUpForm form)
    {
        var memberEntity = new MemberEntity
        {
            UserName = form.Email,
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            PhoneNumber = form.Phone,
        };

        var result = await _userManager.CreateAsync(memberEntity, form.Password);
        return result.Succeeded;

    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }


}
