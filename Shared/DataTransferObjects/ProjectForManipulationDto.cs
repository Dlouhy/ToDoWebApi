using Shared.RequestFeatures.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public abstract class ProjectForManipulationDto
{
    [Required(ErrorMessage = "ProjectName is a required field.")]
    [MaxLength(50, ErrorMessage = "Maximum length for the ProjectName field is 50 characters.")]
    public string ProjectName { get; set; } = string.Empty;

    [MaxLength(250, ErrorMessage = "Maximum length for the ProjectDescription field is 250 characters.")]
    public string ProjectDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "ProjectStart date is a required field.")]
    public DateTime ProjectStart { get; set; }

    [GreaterThan("ProjectStart")]
    public DateTime? ProjectEnd { get; set; }
}