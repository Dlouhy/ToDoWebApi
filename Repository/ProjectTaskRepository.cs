using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly ToDoContext _context;

    public ProjectTaskRepository(ToDoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddProjectTaskAsync(int projectId, ProjectTask projectTask)
    {
        projectTask.ProjectId = projectId;
        await _context.ProjectTasks.AddAsync(projectTask);
    }

    public async Task<ProjectTask> GetProjectTaskAsync(int projectId, int projectTaskId)
    {
        return await _context.ProjectTasks.Where(c => c.ProjectId == projectId && c.Id == projectTaskId).FirstOrDefaultAsync();
    }

    public void DeleteProjectTask(ProjectTask projectTask)
    {
        _context.ProjectTasks.Remove(projectTask);
    }

    public async Task<IEnumerable<ProjectTask>> GetProjectTasksAsync(int projectId)
    {
        return await _context.ProjectTasks.Where(c => c.ProjectId == projectId)
                                          .OrderBy(c => c.ProjectTaskName)
                                          .ToListAsync();
    }
}