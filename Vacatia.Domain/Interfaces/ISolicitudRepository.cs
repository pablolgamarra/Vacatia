using System;
using System.Collections.Generic;
using System.Text;
using Vacatia.Domain.Entities;

namespace Vacatia.Domain.Interfaces
{
    public interface ISolicitudRepository
    {
        Task <Solicitud?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Solicitud>> ObtenerPorUsuarioAsync(string usuarioId, string tenantId, CancellationToken ct = default);
        Task<IReadOnlyList<Solicitud>> ObtenerPendientesAsync(string tenantId, CancellationToken ct = default);
        Task<IReadOnlyList<Solicitud>> ObtenerSolicitudesActivasEnPeriodoAsync(string usuarioId, DateOnly inicio, DateOnly fin, CancellationToken ct = default);
        Task AgregarAsync(Solicitud solicitud, CancellationToken ct = default);
        void Actualizar(Solicitud solicitud);

    }
}
