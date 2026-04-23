using System;
using System.Collections.Generic;
using System.Text;
using Vacatia.Domain.Entities;

namespace Vacatia.Domain.Interfaces
{
    public interface ISaldoVacacionesRepository
    {
        Task<SaldoVacaciones?> ObtenerPorUsuarioAsync(string usuarioId, string tenantId, int anio, CancellationToken ct = default);
        Task AgregarAsync(SaldoVacaciones saldo, CancellationToken ct = default);
        void Actualizar(SaldoVacaciones saldo);
    }
}
