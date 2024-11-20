using Shared.RequestFeatures.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public class ProjectTaskForManipulationDto
{
    [Required(ErrorMessage = "ProjectTask name is a required field.")]
    [MaxLength(50, ErrorMessage = "Maximum length for the ProjectTaskName i field is 50 characters.")]
    public string ProjectTaskName { get; set; } = string.Empty;

    [MaxLength(250, ErrorMessage = "Maximum length for the ProjectTaskDescription field is 250 characters.")]
    public string? ProjectTaskDescription { get; set; }

    [Required(ErrorMessage = "ProjectTaskStart date is a required field.")]
    public DateTime ProjectTaskStart { get; set; }

    [GreaterThan("ProjectTaskStart")]
    public DateTime ProjectTaskEnd { get; set; }

    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
}