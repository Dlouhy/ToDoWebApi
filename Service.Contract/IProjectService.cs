using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contract;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(ProjectForCreationDto projectForCreationDto);

    Task<ProjectDto> GetProjectAsync(int projectId);

    Task<(IEnumerable<ProjectDto> projectDtos, PaginationMetaData paginationMetaData)> GetProjectsAsync(ResourceParameters projectsResourceParameters, IUrlHelper urlHelper);
}