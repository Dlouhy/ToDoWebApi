namespace Contracts;

public interface IUnitOfWork
{
    IProjectRepository ProjectRepository { get; }
    IProjectTaskRepository ProjectTaskRepository { get; }

    Task<bool> SaveAsync();
}