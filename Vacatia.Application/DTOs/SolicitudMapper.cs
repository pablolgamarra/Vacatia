using Vacatia.Domain.Entities;

namespace Vacatia.Application.DTOs
{
    public static class SolicitudMapper
    {
        public static SolicitudDto MapToDto(Solicitud s) => new(
            Id: s.Id,
            UsuarioId: s.UsuarioId,
            UsuarioNombre: s.UsuarioNombre,
            UsuarioEmail: s.UsuarioEmail,
            Tipo: s.Tipo,
            FechaInicio: s.Periodo.FechaInicio,
            FechaFin: s.Periodo.FechaFin,
            DiasConsumidos: s.DiasConsumidos,
            Estado: s.Estado.Valor,
            Motivo: s.Motivo,
            MotivoRechazo: s.MotivoRechazo,
            AprobadorId: s.AprobadorId,
            FechaCreacion: s.FechaCreacion
        );
    }
}
