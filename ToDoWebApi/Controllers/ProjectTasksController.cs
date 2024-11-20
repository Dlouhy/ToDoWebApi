using Asp.Versioning;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Service;
using Shared.DataTransferObjects;

namespace ToDoWebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/projects/{projectId}/projecttasks")]
[ApiVersion(1)]
[ApiVersion(2)]
public class ProjectTasksController : ControllerBase
{
    private readonly ProjectTaskService _projectTaskService;

    public ProjectTasksController(ProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetProjectTasksForProjectAsync(int projectId)
    {
        var projectTasks = await _projectTaskService.GetProjectTasksAsync(projectId);
        return Ok(projectTasks);
    }

    [HttpGet("{projectTaskId}", Name = "GetProjectTaskForProject")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [MapToApiVersion(2)]
    public async Task<ActionResult<ProjectTaskDto>> GetProjectTaskForProjectAsync(int projectId, int projectTaskId)
    {
        var projectTaskDto = await _projectTaskService.GetProjectTaskForProjectAsync(projectId, projectTaskId);

        return Ok(projectTaskDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProjectTaskDto>> CreateProjectTaskForProjectAsync(int projectId, ProjectTaskForCreationDto taskForCreationDto)
    {
        var projectTaskToReturn = await _projectTaskService.CreateProjectTaskForProjectAsync(projectId, taskForCreationDto);

        return CreatedAtRoute("GetProjectTaskForProject", new { projectId, projectTaskId = projectTaskToReturn.Id }, projectTaskToReturn);
    }

    [HttpPut("{projectTaskId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProjectTaskForProjectAsync(int projectId, int projectTaskId, ProjectTaskForUpdateDto taskForUpdateDto)
    {
        await _projectTaskService.UpdateProjectTaskForProjectAsync(projectId, projectTaskId, taskForUpdateDto);
        return NoContent();
    }

    [HttpPatch("{projectTaskId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [MapToApiVersion(2)]
    public async Task<IActionResult> PartiallyUpdateProjectTaskForProjectAsync(int projectId, int projectTaskId, JsonPatchDocument<ProjectTaskForUpdateDto> patchDocument)
    {
        if (patchDocument is null)
        {
            return BadRequest("patchDocument object sent from client is null.");
        }

        var (projectTaskToPatch, projectTaskEntity) = await _projectTaskService.GetProjectTaskForPatchAsync(projectId, projectTaskId);

        patchDocument.ApplyTo(projectTaskToPatch, ModelState);

        if (!TryValidateModel(projectTaskToPatch))
        {
            return ValidationProblem(ModelState);
        }

        await _projectTaskService.SaveChangesForPatchAsync(projectTaskToPatch, projectTaskEntity);

        return NoContent();
    }

    [HttpDelete("{projectTaskId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteProjectTaskForProjectAsync(int projectId, int projectTaskId)
    {
        await _projectTaskService.DeleteProjectTaskForProjectAsync(projectId, projectTaskId);

        return NoContent();
    }

    public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
    {
        var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

        return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
    }
}