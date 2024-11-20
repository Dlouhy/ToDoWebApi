using Entities;
using Shared.DataTransferObjects;

namespace Service.Contract;

public interface IProjectTaskService
{
    Task<ProjectTaskDto> CreateProjectTaskForProjectAsync(int projectId, ProjectTaskForCreationDto taskForCreationDto);

    Task DeleteProjectTaskForProjectAsync(int projectId, int projectTaskId);

    Task<(ProjectTaskForUpdateDto projectTaskToPatch, ProjectTask projectTaskEntity)> GetProjectTaskForPatchAsync(int projectId, int projectTaskId);

    Task<ProjectTaskDto> GetProjectTaskForProjectAsync(int projectId, int projectTaskId);

    Task<IEnumerable<ProjectTaskDto>> GetProjectTasksAsync(int projectId);

    Task SaveChangesForPatchAsync(ProjectTaskForUpdateDto projectTaskToPatch, ProjectTask projectTaskEntity);

    Task UpdateProjectTaskForProjectAsync(int projectId, int projectTaskId, ProjectTaskForUpdateDto taskForUpdateDto);
}