using Entities;

namespace Contracts;

public interface IProjectTaskRepository
{
    Task AddProjectTaskAsync(int projectId, ProjectTask projectTask);

    void DeleteProjectTask(ProjectTask projectTask);

    Task<ProjectTask> GetProjectTaskAsync(int projectId, int projectTaskId);

    Task<IEnumerable<ProjectTask>> GetProjectTasksAsync(int projectId);
}