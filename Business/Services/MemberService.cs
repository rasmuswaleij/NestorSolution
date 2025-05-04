//Har bytt ut MemberEntity till IdentityRole på RoleManager<> i konstruktorn samt bytt ut rolemanager till usermanager på uppmaning av AI

using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business.Services;

public class MemberService(UserManager<MemberEntity> userManager, IMemberRepository memberRepository, RoleManager<IdentityRole> roleManager) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;


    //Det är bara den nedersta som används för tillfället. Behöver ändra längre fram!
    public async Task<MemberResult> GetMembersAsync()
    {
        var result = await _memberRepository.GetAllAsync();
        return result.MapTo<MemberResult>();

    }

    public async Task<MemberResult> AddUserAsync(MemberSignUpForm form)
    {
        var entity = form.MapTo<MemberEntity>();

        var result = await _userManager.CreateAsync(entity);

        return result.MapTo<MemberResult>();
    }




    public async Task<MemberResult> CreateUserAsync(SignUpFormData formData, string roleName = "User")
    {
        if (formData == null)
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Form data can not be null" };

        var existsResult = await _memberRepository.Exists(x => x.Email == formData.Email);
        if (existsResult.Succeeded)
            return new MemberResult { Succeeded = false, StatusCode = 409, Error = "User with entered email already exists" };

        try
        {
            var userEntity = formData.MapTo<MemberEntity>();

            var result = await _userManager.CreateAsync(userEntity, formData.Password);
            if (result.Succeeded)
            {
                var addToRoleResult = await AddUserToRole(userEntity.Id, roleName);
                return result.Succeeded
                   ? new MemberResult { Succeeded = true, StatusCode = 201 }
                   : new MemberResult { Succeeded = false, StatusCode = 201, Error = "User created but not added to role" };
            }

            return new MemberResult { Succeeded = false, StatusCode = 500, Error = "Unable to create user" };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }






    //public async Task<MemberResult> AddUserToRole(string memberId, string roleName)
    //{
    //    if (!await _roleManager.RoleExistsAsync(roleName))
    //        return new MemberResult { Succeeded = false, StatusCode = 404, Error = "Role does not exist." };

    //    var user = await _roleManager.FindByIdAsync(memberId);
    //    if (user == null)
    //        return new MemberResult { Succeeded = false, StatusCode = 404, Error = "User does not exist." };


    //    var result = await _userManager.AddToRoleAsync(user, roleName);

    //    return result.Succeeded
    //        ? new MemberResult { Succeeded = true, StatusCode = 200 }
    //        : new MemberResult { Succeeded = false, StatusCode = 500, Error = "Error. Unable to add user to role." };
    //}

    //AI-genererad omvandling av metoden ovan
    public async Task<MemberResult> AddUserToRole(string memberId, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
            return new MemberResult { Succeeded = false, StatusCode = 404, Error = "Role does not exist." };

        var user = await _userManager.FindByIdAsync(memberId); // Ändrat från _roleManager till _userManager
        if (user == null)
            return new MemberResult { Succeeded = false, StatusCode = 404, Error = "User does not exist." };

        var result = await _userManager.AddToRoleAsync(user, roleName);

        return result.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 200 }
            : new MemberResult { Succeeded = false, StatusCode = 500, Error = "Error. Unable to add user to role." };
    }

    public async Task<IEnumerable<Member>> GetAllMembers()
    {
        var list = await _userManager.Users.ToListAsync();
        var members = list.Select(x => new Member
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            Phone = x.PhoneNumber,
            JobTitle = x.JobTitle,

        });
        return members;
    }
}
