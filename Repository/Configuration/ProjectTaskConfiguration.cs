using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ToTable("ProjectTask")
                  .HasData(
                    new ProjectTask()
                    {
                        Id = 1,
                        ProjectTaskName = "Frontend Development",
                        ProjectTaskDescription = "Develop the user interface and user experience.",
                        ProjectTaskStart = new DateTime(2023, 11, 16),
                        ProjectTaskEnd = new DateTime(2023, 12, 15),
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = 1
                    },
                    new ProjectTask()
                    {
                        Id = 2,
                        ProjectTaskName = "Backend Development",
                        ProjectTaskDescription = "Implement the server-side logic and database interactions.",
                        ProjectTaskStart = new DateTime(2023, 11, 20),
                        ProjectTaskEnd = new DateTime(2024, 01, 15),
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = 1
                    },
                    new ProjectTask()
                    {
                        Id = 3,
                        ProjectTaskName = "Design User Interface",
                        ProjectTaskDescription = "Create wireframes and mockups for the website's user interface.",
                        ProjectTaskStart = new DateTime(2023, 11, 27),
                        ProjectTaskEnd = new DateTime(2023, 12, 10),
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = 2
                    },
                    new ProjectTask()
                    {
                        Id = 4,
                        ProjectTaskName = "Develop Mobile App Backend",
                        ProjectTaskDescription = "Build the REST API for the mobile app.",
                        ProjectTaskStart = new DateTime(2023, 12, 03),
                        ProjectTaskEnd = new DateTime(2024, 01, 15),
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = 3
                    });
    }
}