using Microsoft.EntityFrameworkCore;
using Vacatia.Domain.Entities;
using Vacatia.Domain.Interfaces;
using Vacatia.Domain.ValueObjects;

namespace Vacatia.Infraestructure.Persistance.Repositories
{
    public class SolicitudRepository : ISolicitudRepository
    {
        private readonly AppDbContext _db;

        public SolicitudRepository (AppDbContext db)
        {
            _db = db;
        }

        public void Actualizar(Solicitud solicitud)
        {
            _db.Solicitudes.Update(solicitud);
        }

        public async Task AgregarAsync(Solicitud solicitud, CancellationToken ct = default)
        {
            await _db.Solicitudes.AddAsync(solicitud, ct);
        }

        public async Task<IReadOnlyList<Solicitud>> ObtenerPendientesAsync(string tenantId, CancellationToken ct = default)
        {
            return await _db.Solicitudes.Where(s => s.Estado == EstadoSolicitud.Pendiente && s.TenantId == tenantId).OrderBy(s => s.FechaCreacion).ToListAsync(ct);
        }

        public async Task<Solicitud?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Solicitudes.FindAsync([id], ct);
        }

        public async Task<IReadOnlyList<Solicitud>> ObtenerPorUsuarioAsync(string usuarioId, string tenantId, CancellationToken ct = default)
        {
            return await _db.Solicitudes.Where(s => s.UsuarioId == usuarioId && s.TenantId == tenantId).OrderByDescending(s => s.FechaCreacion).ToListAsync(ct);
        }

        public async Task<IReadOnlyList<Solicitud>> ObtenerSolicitudesActivasEnPeriodoAsync(string usuarioId, DateOnly inicio, DateOnly fin, CancellationToken ct = default)
        {
            return await _db.Solicitudes.Where(s => s.UsuarioId == usuarioId && s.Estado != EstadoSolicitud.Rechazado && s.Estado != EstadoSolicitud.Cancelado && s.Periodo.FechaInicio <= fin && s.Periodo.FechaFin >= inicio).ToListAsync(ct);
        }
    }
}
