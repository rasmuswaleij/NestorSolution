using Business.Interfaces;
using Business.Services;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebApp.Controllers;

[Route("Admin/Projects")]
public class ProjectsController(IProjectService projectService) : Controller
{

    private readonly IProjectService _projectService = projectService;

    public IActionResult Index()
    {

        return View();
    }

    [HttpGet("ShowAllProjects")]
    public  async Task<IActionResult> ShowAllProjects()
    {
        var projects = await _projectService.GetProjectsAsync();


        return View(projects);
    }
    [HttpGet("ShowStartedProjects")]
    public async Task<IActionResult> ShowStartedProjects()
    {
        var projects = await _projectService.GetStartedProjectsAsync();

        return View(projects);
    }
    [HttpGet("ShowCompletedProjects")]
    public async Task<IActionResult> ShowCompletedProjects()
    {
        var projects = await _projectService.GetCompletedProjectsAsync();

        return View(projects);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromForm] ProjectAddFormData form)
    {
        if (!ModelState.IsValid)
        {
            //return PartialView("Partials/Sections/_AddProjectForm", form);
            return View("ShowAllProjects", form);
        }


        //send data to clientService
        var result = await _projectService.CreateProjectAsync(form);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Something went wrong");
            //return View("Add", form); // eller visa ett felmeddelande
            return RedirectToAction("ShowAllProjects", "Projects");

        }

        return RedirectToAction("ShowAllProjects", "Projects");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        Console.WriteLine("Project ID" + id);
        Debug.WriteLine("Project ID" + id);


        var project = await _projectService.GetProjectAsync(id);
        if (project == null)
        {
            return NotFound();
        }



        var form = new EditProjectForm
        {
            Id = project.Result.Id,
            ProjectName = project.Result.ProjectName,
            Description = project.Result.Description,
            StartDate = project.Result.StartDate,
            EndDate = project.Result.EndDate,
            Budget = project.Result.Budget,
            ClientId = project.Result.ClientId ?? "",
            MemberId = project.Result.MemberId ?? "",
            StatusId = project.Result.StatusId,


        };


        return PartialView("Partials/Sections/_EditProjectModal", form);
    }


    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] EditProjectForm form)
    {

        Console.WriteLine("Project form" + form);
        Debug.WriteLine("Project form" + form);

        //send data to clientService
        var result = await _projectService.UpdateProjectAsync(form);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Something went wrong");
            return View(form); // eller visa ett felmeddelande
        }

        return RedirectToAction("Projects", "Admin");
    }

    [HttpPost]
    public IActionResult Delete()
    {
        return View();
    }
}
