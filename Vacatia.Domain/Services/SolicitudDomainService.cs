using System;
using System.Collections.Generic;
using System.Text;
using Vacatia.Domain.Exceptions;
using Vacatia.Domain.Interfaces;
using Vacatia.Domain.ValueObjects;

namespace Vacatia.Domain.Services
{
    /// <summary>
    /// Servicio de dominio para operaciones que involucran múltiples agregados.
    /// </summary>
    public class SolicitudDomainService
    {
        private readonly ISolicitudRepository _solicitudRepository;

        public SolicitudDomainService(ISolicitudRepository solicitudRepository)
        {
            this._solicitudRepository = solicitudRepository;
        }

        /// <summary>
        /// Valida que no exista superposición de fechas con otras solicitudes activas del usuario.
        /// </summary>
        public async Task ValidarSuperposicionAsync(string usuarioId, PeriodoFechas periodo, CancellationToken ct = default)
        {
            var solicitudesActivas = await _solicitudRepository.ObtenerSolicitudesActivasEnPeriodoAsync(usuarioId, periodo.FechaInicio, periodo.FechaFin, ct);

            if (solicitudesActivas.Any())
            {
                throw new DomainException("Ya hay una solicitud activa que se superpone con el periodo solicitado.");
            }
        }
    }
}
