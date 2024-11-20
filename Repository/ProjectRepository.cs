using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;
using Shared.PropertyMapping;
using Shared.RequestFeatures;

namespace Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly ToDoContext _context;

    private readonly IPropertyMappingService _propertyMappingService;

    public ProjectRepository(ToDoContext context, IPropertyMappingService propertyMappingService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
    }

    public async Task AddProjectAsync(Project project)
    {
        await _context.Projects.AddAsync(project).ConfigureAwait(false);
    }

    public async Task<bool> ProjectExistsAsync(int projectId)
    {
        return await _context.Projects.AnyAsync(a => a.Id == projectId);
    }

    public void DeleteProject(Project project)
    {
        _context.Projects.Remove(project);
    }

    public async Task<Project> GetProjectAsync(int projectId)
    {
        return await _context.Projects.Include(c => c.ProjectTasks).Where(c => c.Id == projectId).FirstOrDefaultAsync();
    }

    public async Task<PagedList<Project>> GetProjectsAsync(ResourceParameters projectsResourceParameters)
    {
        //if (string.IsNullOrWhiteSpace(projectsResourceParameters.MainCategory)
        //    && string.IsNullOrWhiteSpace(projectsResourceParameters.SearchQuery))
        //{
        //    return await GetAuthorsAsync();
        //}

        IQueryable<Project> collection = _context.Projects.Include(c => c.ProjectTasks);

        if (!string.IsNullOrWhiteSpace(projectsResourceParameters.SearchQuery))
        {
            string searchQuery = projectsResourceParameters.SearchQuery.Trim();
            collection = collection.Where(a => a.ProjectName.Contains(searchQuery)
               || (a.ProjectDescription != null && a.ProjectDescription.Contains(searchQuery)));
        }

        if (!string.IsNullOrWhiteSpace(projectsResourceParameters.OrderBy))
        {
            Dictionary<string, PropertyMappingValue> projectPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<ProjectDto, Project>();

            collection = collection.ApplySort(projectsResourceParameters.OrderBy, projectPropertyMappingDictionary);
        }

        return await PagedList<Project>.CreateAsync(collection, projectsResourceParameters.PageNumber, projectsResourceParameters.PageSize).ConfigureAwait(false);
    }
}