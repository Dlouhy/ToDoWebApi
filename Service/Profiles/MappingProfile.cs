using AutoMapper;
using Entities;
using Shared.DataTransferObjects;

namespace Service.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectForCreationDto, Project>();

        CreateMap<ProjectTask, ProjectTaskDto>();
        CreateMap<ProjectTaskForCreationDto, ProjectTask>();
        CreateMap<ProjectTaskForUpdateDto, ProjectTask>();

        CreateMap<UserForRegistrationDto, User>();
    }
}