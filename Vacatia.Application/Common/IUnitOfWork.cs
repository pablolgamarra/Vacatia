namespace Vacatia.Application.Common
{
    public interface IUnitOfWork
    {
        Task<int> GuardarCambiosAsync(CancellationToken ct = default);
    }
}
