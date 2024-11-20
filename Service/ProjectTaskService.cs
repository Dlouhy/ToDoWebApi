using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Service.Contract;
using Shared.DataTransferObjects;

namespace Service;

public class ProjectTaskService : IProjectTaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProjectTaskService(IUnitOfWork repository, IMapper mapper)
    {
        _unitOfWork = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectTaskDto>> GetProjectTasksAsync(int projectId)
    {
        if (!await _unitOfWork.ProjectRepository.ProjectExistsAsync(projectId))
        {
            throw new ProjectNotFoundException(projectId);
        }

        IEnumerable<ProjectTask> tasksForProject = await _unitOfWork.ProjectTaskRepository.GetProjectTasksAsync(projectId);

        return _mapper.Map<IEnumerable<ProjectTaskDto>>(tasksForProject);
    }

    public async Task<ProjectTaskDto> GetProjectTaskForProjectAsync(int projectId, int projectTaskId)
    {
        if (!await _unitOfWork.ProjectRepository.ProjectExistsAsync(projectId))
        {
            throw new ProjectNotFoundException(projectId);
        }

        var projectTaskForProjectFromRepo = await _unitOfWork.ProjectTaskRepository.GetProjectTaskAsync(projectId, projectTaskId);

        if (projectTaskForProjectFromRepo is null)
        {
            throw new ProjectTaskNotFoundException(projectId, projectTaskId);
        }

        return _mapper.Map<ProjectTaskDto>(projectTaskForProjectFromRepo);
    }

    //TODO: check null ProjectTaskForCreationDto, muze to byt valstne null?
    //zkusit z postmann a swagger
    public async Task<ProjectTaskDto> CreateProjectTaskForProjectAsync(int projectId, ProjectTaskForCreationDto taskForCreationDto)
    {
        if (!await _unitOfWork.ProjectRepository.ProjectExistsAsync(projectId))
        {
            throw new ProjectNotFoundException(projectId);
        }

        var projectTaskEntity = _mapper.Map<ProjectTask>(taskForCreationDto);

        ArgumentNullException.ThrowIfNull(projectTaskEntity);

        await _unitOfWork.ProjectTaskRepository.AddProjectTaskAsync(projectId, projectTaskEntity);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ProjectTaskDto>(projectTaskEntity);
    }

    public async Task UpdateProjectTaskForProjectAsync(int projectId, int projectTaskId, ProjectTaskForUpdateDto taskForUpdateDto)
    {
        if (!await _unitOfWork.ProjectRepository.ProjectExistsAsync(projectId))
        {
            throw new ProjectNotFoundException(projectId);
        }

        var projectTaskForProjectFromRepo = await _unitOfWork.ProjectTaskRepository.GetProjectTaskAsync(projectId, projectTaskId);

        if (projectTaskForProjectFromRepo is null)
        {
            throw new ProjectTaskNotFoundException(projectId, projectTaskId);
        }

        _mapper.Map(taskForUpdateDto, projectTaskForProjectFromRepo);

        await _unitOfWork.SaveAsync();
    }

    public async Task<(ProjectTaskForUpdateDto projectTaskToPatch, ProjectTask projectTaskEntity)> GetProjectTaskForPatchAsync(int projectId, int projectTaskId)
    {
        if (!await _unitOfWork.ProjectRepository.ProjectExistsAsync(projectId))
        {
            throw new ProjectNotFoundException(projectId);
        }

        var projectTaskForProjectFromRepo = await _unitOfWork.ProjectTaskRepository.GetProjectTaskAsync(projectId, projectTaskId);

        if (projectTaskForProjectFromRepo is null)
        {
            throw new ProjectTaskNotFoundException(projectId, projectTaskId);
        }

        var projectTaskToPatch = _mapper.Map<ProjectTaskForUpdateDto>(projectTaskForProjectFromRepo);

        return (projectTaskToPatch, projectTaskForProjectFromRepo);
    }

    public async Task SaveChangesForPatchAsync(ProjectTaskForUpdateDto projectTaskToPatch, ProjectTask projectTaskEntity)
    {
        _mapper.Map(projectTaskToPatch, projectTaskEntity);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteProjectTaskForProjectAsync(int projectId, int projectTaskId)
    {
        if (!await _unitOfWork.ProjectRepository.ProjectExistsAsync(projectId))
        {
            throw new ProjectNotFoundException(projectId);
        }

        var projectTaskForProjectFromRepo = await _unitOfWork.ProjectTaskRepository.GetProjectTaskAsync(projectId, projectTaskId);

        if (projectTaskForProjectFromRepo is null)
        {
            throw new ProjectTaskNotFoundException(projectId, projectTaskId);
        }

        _unitOfWork.ProjectTaskRepository.DeleteProjectTask(projectTaskForProjectFromRepo);
        await _unitOfWork.SaveAsync();
    }
}