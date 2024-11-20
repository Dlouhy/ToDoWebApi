using Contracts;

namespace Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ToDoContext _context;
    public IProjectRepository ProjectRepository { get; }
    public IProjectTaskRepository ProjectTaskRepository { get; }

    public UnitOfWork(ToDoContext context, IProjectRepository projectRepository, IProjectTaskRepository projectTaskRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        ProjectRepository = projectRepository;
        ProjectTaskRepository = projectTaskRepository;
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}