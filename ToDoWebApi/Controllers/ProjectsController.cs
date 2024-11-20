using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace ToDoWebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/projects")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ApiVersion(1)]
[ApiVersion(2)]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
    }

    [HttpGet(Name = "GetProjects")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjectsAsync([FromQuery] ResourceParameters projectsResourceParameters)
    {
        var (projectDtos, paginationMetaData) = await _projectService.GetProjectsAsync(projectsResourceParameters, Url);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

        return Ok(projectDtos);
    }

    [HttpGet("{projectId}", Name = "GetProject")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [MapToApiVersion(2)]
    public async Task<ActionResult<ProjectDto>> GetProjectAsync(int projectId)
    {
        var project = await _projectService.GetProjectAsync(projectId);

        return Ok(project);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProjectDto>> CreateProjectAsync(ProjectForCreationDto projectForCreation)
    {
        ProjectDto projectDto = await _projectService.CreateProjectAsync(projectForCreation);

        return CreatedAtRoute("GetProject", new { projectId = projectDto.Id }, projectDto);
    }

    [HttpOptions()]
    public IActionResult GetProjectsOptions()
    {
        Response.Headers.Append("Allow", "GET,HEAD,POST,OPTIONS");
        return Ok();
    }
}