using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using Shared.DataTransferObjects;
using Shared.PropertyMapping;
using Shared.RequestFeatures;

namespace Service;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPropertyMappingService _propertyMappingService;

    public ProjectService(IUnitOfWork repository, IMapper mapper, IPropertyMappingService propertyMappingService)
    {
        _unitOfWork = repository;
        _mapper = mapper;
        _propertyMappingService = propertyMappingService ??
        throw new ArgumentNullException(nameof(propertyMappingService));
    }

    public async Task<ProjectDto> GetProjectAsync(int projectId)
    {
        var project = await _unitOfWork.ProjectRepository.GetProjectAsync(projectId).ConfigureAwait(false);

        if (project is null)
        {
            throw new ProjectNotFoundException(projectId);
        }

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<(IEnumerable<ProjectDto> projectDtos, PaginationMetaData paginationMetaData)> GetProjectsAsync(ResourceParameters projectsResourceParameters, IUrlHelper urlHelper)
    {
        if (projectsResourceParameters is null)
        {
            throw new ResourceParametersBadRequestException();
        }

        if (!_propertyMappingService.ValidMappingExistsFor<ProjectDto, Project>(projectsResourceParameters.OrderBy))
        {
            throw new NotValidMappingsBadRequestException(projectsResourceParameters.OrderBy);
        }

        var projects = await _unitOfWork.ProjectRepository.GetProjectsAsync(projectsResourceParameters);

        string? previousPageLink;
        if (projects.HasPrevious)
        {
            previousPageLink = CreateProjectsResourceUri(projectsResourceParameters, ResourceUriType.PreviousPage, urlHelper);
        }
        else
        {
            previousPageLink = null;
        }

        string? nextPageLink;
        if (projects.HasNext)
        {
            nextPageLink = CreateProjectsResourceUri(projectsResourceParameters, ResourceUriType.NextPage, urlHelper);
        }
        else
        {
            nextPageLink = null;
        }

        var paginationMetadata = new PaginationMetaData
        {
            TotalCount = projects.TotalCount,
            PageSize = projects.PageSize,
            CurrentPage = projects.CurrentPage,
            TotalPages = projects.TotalPages,
            PreviousPageLink = previousPageLink,
            NextPageLink = nextPageLink
        };

        var projectDtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);

        return (projectDtos, paginationMetadata);
    }

    private static string? CreateProjectsResourceUri(ResourceParameters projectsResourceParameters, ResourceUriType type, IUrlHelper urlHelper)
    {
        switch (type)
        {
            case ResourceUriType.PreviousPage:
                return urlHelper.Link("GetProjects",
                    new
                    {
                        orderBy = projectsResourceParameters.OrderBy,
                        pageNumber = projectsResourceParameters.PageNumber - 1,
                        pageSize = projectsResourceParameters.PageSize,
                        searchQuery = projectsResourceParameters.SearchQuery
                    });

            case ResourceUriType.NextPage:
                return urlHelper.Link("GetProjects",
                    new
                    {
                        orderBy = projectsResourceParameters.OrderBy,
                        pageNumber = projectsResourceParameters.PageNumber + 1,
                        pageSize = projectsResourceParameters.PageSize,
                        searchQuery = projectsResourceParameters.SearchQuery
                    });

            default:
                return urlHelper.Link("GetProjects",
                    new
                    {
                        orderBy = projectsResourceParameters.OrderBy,
                        pageNumber = projectsResourceParameters.PageNumber,
                        pageSize = projectsResourceParameters.PageSize,
                        searchQuery = projectsResourceParameters.SearchQuery
                    });
        }
    }

    public async Task<ProjectDto> CreateProjectAsync(ProjectForCreationDto projectForCreationDto)
    {
        var projectEntity = _mapper.Map<Project>(projectForCreationDto);

        await _unitOfWork.ProjectRepository.AddProjectAsync(projectEntity).ConfigureAwait(false);
        await _unitOfWork.SaveAsync().ConfigureAwait(false);

        return _mapper.Map<ProjectDto>(projectEntity);
    }
}