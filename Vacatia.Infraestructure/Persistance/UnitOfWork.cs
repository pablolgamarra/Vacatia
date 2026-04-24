using Vacatia.Application.Common.Interfaces;

namespace Vacatia.Infraestructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context) => _context = context;
        
        public Task<int> GuardarCambiosAsync(CancellationToken ct=default)
        {
            return _context.SaveChangesAsync(ct);
        }
    }
}
