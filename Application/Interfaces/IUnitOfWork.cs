namespace Application.Interfaces;

public interface IUnitOfWork
{
    Task<int>     SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action);
}