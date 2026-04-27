using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Vacatia.Domain.Entities;
using Vacatia.Domain.Interfaces;
using Vacatia.Domain.ValueObjects;

namespace Vacatia.Infraestructure.Persistance.Repositories
{
    internal class SaldoVacacionesRepository : ISaldoVacacionesRepository
    {
        private readonly AppDbContext _db;

        public SaldoVacacionesRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AgregarAsync(SaldoVacaciones saldovacaciones, CancellationToken ct = default)
        {
            await _db.SaldosVacaciones.AddAsync(saldovacaciones, ct);
        }

        public void Actualizar(SaldoVacaciones saldo) => _db.SaldosVacaciones.Update(saldo);

        public Task<SaldoVacaciones?> ObtenerPorUsuarioAsync(string usuarioId, string tenantId, int anio, CancellationToken ct = default) 
            => _db.SaldosVacaciones.Where(s => s.UsuarioId == usuarioId && s.TenantId == tenantId && s.Anio == anio).FirstOrDefaultAsync(ct);
    }
}
