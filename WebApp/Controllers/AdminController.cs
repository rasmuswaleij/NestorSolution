using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebApp.Controllers;

[Authorize]
public class AdminController(IMemberService memberService, IProjectService projectService, IStatusService statusService) : Controller
{

    private readonly IMemberService _memberService = memberService;
    private readonly IProjectService _projectService = projectService;
    private readonly IStatusService _statusService = statusService;



    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Projects()
    {
        Console.WriteLine("Testsrtetst");
        Debug.WriteLine("Testsrtetst");

        var statuses = await _statusService.GetStatusesAsync();
        ViewBag.Statuses = statuses;


        var projects = await _projectService.GetProjectsAsync();

        return View(projects);
    }
    public async Task<IActionResult> Members()
    {
        var members = await _memberService.GetAllMembers();

        return View(members);
    }
    public IActionResult Clients()
    {
        return View();
    }
}
