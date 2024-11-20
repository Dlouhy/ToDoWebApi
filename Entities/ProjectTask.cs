using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class ProjectTask
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "ProjectTask name is a required field.")]
    [MaxLength(50, ErrorMessage = "Maximum length for the ProjectTaskName i field is 50 characters.")]
    public string ProjectTaskName { get; set; } = string.Empty;

    [MaxLength(250, ErrorMessage = "Maximum length for the ProjectTaskDescription field is 250 characters.")]
    public string? ProjectTaskDescription { get; set; }

    [Required(ErrorMessage = "ProjectTaskStart date is a required field.")]
    public DateTime ProjectTaskStart { get; set; }

    public DateTime? ProjectTaskEnd { get; set; }

    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }

    [ForeignKey("ProjectId")]
    public Project Project { get; set; } = null!;

    public int ProjectId { get; set; }
}