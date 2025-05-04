using Business.Models;
using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResult> CreateProjectAsync(ProjectAddFormData formData);
        Task<ProjectResult<Project>> GetProjectAsync(string id);
        Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();

        Task<ProjectResult> UpdateProjectAsync(EditProjectForm formData);

        //Task<IEnumerable<ProjectEntity>> GetProjectByStatusAsync(int? statusId);
    }
}