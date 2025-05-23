﻿using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    public async Task<ProjectResult> CreateProjectAsync(ProjectAddFormData formData)
    {
        Console.WriteLine(">> Create called");
        Debug.WriteLine(">> Create called");


        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fileds are supplied" };

        var projectEntity = formData.MapTo<ProjectEntity>();

        //Nedan ska inte vara statisk men testar med 1
        var statusResult = await _statusService.GetStatusByIdAsync(1);

        //Generated by AI
        if (!statusResult.Succeeded || statusResult.Result == null)
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 500,
                Error = "Could not find status"
            };
        }

        projectEntity.StatusId = statusResult.Result.Id;

        //
        Console.WriteLine(">> AddAsync called");
        Debug.WriteLine(">> AddAsync called");
        var result = await _projectRepository.AddAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };

    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (orderByDescending: true,
            sortBy: s => s.Created,
            where: null,
            include => include.Member,
            include => include.Status,
            include => include.Client);

        return response.MapTo<ProjectResult<IEnumerable<Project>>>();
    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetStartedProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (orderByDescending: true,
            sortBy: s => s.Created,
            where: x => x.IsCompleted,
            include => include.Member,
            include => include.Status,
            include => include.Client);

        return response.MapTo<ProjectResult<IEnumerable<Project>>>();
    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetCompletedProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (orderByDescending: true,
            sortBy: s => s.Created,
            where: x => x.IsCompleted == false,
            include => include.Member,
            include => include.Status,
            include => include.Client);

        return response.MapTo<ProjectResult<IEnumerable<Project>>>();
    }


    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        Console.WriteLine("GetProjectAsync started");
        Debug.WriteLine("GetProjectAsync started");

        var response = await _projectRepository.GetAsync
            (where: x => x.Id == id,
            include => include.Member,
            include => include.Status,
            include => include.Client
            );
        return response.Succeeded
            ? new ProjectResult<Project>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = response.Result
            }
            : new ProjectResult<Project>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = $"Project '{id}' was not found."
            };

    }

    public async Task<ProjectResult> UpdateProjectAsync(EditProjectForm formData)

    {

        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fileds are supplied" };

        var projectEntity = formData.MapTo<ProjectEntity>();

        //Nedan ska inte vara statisk men testar med 1
        var statusResult = await _statusService.GetStatusByIdAsync(formData.StatusId);

        //Generated by AI
        if (!statusResult.Succeeded || statusResult.Result == null)
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 500,
                Error = "Could not find status"
            };
        }

        projectEntity.StatusId = statusResult.Result.Id;

  
        var result = await _projectRepository.UpdateAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };

    }

    //public async Task<IEnumerable<Project>> GetProjectByStatusAsync(int? statusId)
    //{
    //    if (statusId == null)
    //    {
    //        var entities = await _projectRepository.GetAllProjectsAsync();
    //        return entities.MapTo<Project>();
    //        // Hämtar alla projekt om inget statusId är valt
    //    }

    //    var entitiesStatus = await _projectRepository.GetProjectsByStatusIdAsync(statusId.Value);
    //    return entitiesStatus.MapTo<Project>();

    //}

   
}