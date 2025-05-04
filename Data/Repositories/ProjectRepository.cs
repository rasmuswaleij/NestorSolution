using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{
    Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync();

    Task<IEnumerable<ProjectEntity>> GetProjectsByStatusIdAsync(int statusId);

}
public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{


    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        return await context.Projects.ToListAsync();
    }

    public async Task<IEnumerable<ProjectEntity>> GetProjectsByStatusIdAsync(int statusId)
    {
        return await context.Projects
            .Where(p => p.StatusId == statusId)
            .ToListAsync();
    }

}
