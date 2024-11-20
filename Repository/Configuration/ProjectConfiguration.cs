using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        //in HasData method has to be Id set
        builder.ToTable("Project")
               .HasData(
                new Project()
                {
                    Id = 1,
                    ProjectName = "Web Application Development",
                    ProjectDescription = "Build a modern web application using ASP.NET Core and React.",
                    ProjectStart = new DateTime(2023, 11, 16),
                    ProjectEnd = new DateTime(2024, 03, 31)
                },
                new Project()
                {
                    Id = 2,
                    ProjectName = "Mobile App Development",
                    ProjectDescription = "Create a cross-platform mobile app using Xamarin.",
                    ProjectStart = new DateTime(2023, 12, 01),
                    ProjectEnd = new DateTime(2024, 05, 31)
                },
                new Project()
                {
                    Id = 3,
                    ProjectName = "Legacy System Migration",
                    ProjectDescription = "Migrate a critical legacy system to a modern cloud-based solution.",
                    ProjectStart = new DateTime(2022, 01, 01),
                    ProjectEnd = new DateTime(2023, 06, 30)
                },
                new Project()
                {
                    Id = 4,
                    ProjectName = "Integration testing",
                    ProjectStart = DateTime.Now
                });
    }
}