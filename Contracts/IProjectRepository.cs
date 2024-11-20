using Entities;
using Shared.RequestFeatures;

namespace Contracts;

public interface IProjectRepository
{
    Task AddProjectAsync(Project project);

    void DeleteProject(Project project);

    Task<Project> GetProjectAsync(int projectId);

    Task<PagedList<Project>> GetProjectsAsync(ResourceParameters projectsResourceParameters);

    Task<bool> ProjectExistsAsync(int projectId);
}