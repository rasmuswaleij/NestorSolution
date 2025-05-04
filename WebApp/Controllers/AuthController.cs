using Business.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;
public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    public IActionResult Login(string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(MemberLoginForm form, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";

        if (ModelState.IsValid) 
        {
            var result = await _authService.LoginAsync(form);
            if(result)
                return LocalRedirect(returnUrl);

        }
        
        ViewBag.ErrorMessage = "Incorrect email or password";
        return View(form);
    }

    

    public IActionResult SignUp()
    {
        return View();
    }


    //public IActionResult SignUp()
    //{
    //    ViewBag.ErrorMessage = "";
    //    return View();
    //}

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
            return View(model);

        var signUpFormData = model.MapTo<SignUpFormData>();
        var result = await _authService.SignUpAsync2(signUpFormData);
        if (result.Succeeded)
            return RedirectToAction("SignIn", "Auth");

        ViewBag.ErrorMessage = result.Error;
        return View(model);
    }

    //[HttpPost]
    //public async Task<IActionResult> SignUp(MemberSignUpForm form)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var result = await _authService.SignUpAsync(form);
    //        if (result)
    //            return LocalRedirect("~/");

    //    }
    //    ViewBag.ErrorMessage = "";
    //    return View(form);
    //}

    public IActionResult SignIn(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = null;
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var signInFormData = model.MapTo<SignInFormData>();
        var result = await _authService.SignInAsync(signInFormData);
        if (result.Succeeded)
            return LocalRedirect(returnUrl);

        ViewBag.ErrorMessage = result.Error;
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return LocalRedirect("~/");
    }
}
