using Vacatia.Domain.Enums;

namespace Vacatia.Application.DTOs
{
    public record SolicitudDto(
        Guid Id,
        string UsuarioId,
        string UsuarioNombre,
        string UsuarioEmail,
        TipoSolicitud Tipo,
        DateOnly FechaInicio,
        DateOnly FechaFin,
        int DiasConsumidos,
        string Estado,
        string? Motivo,
        string? MotivoRechazo,
        string? AprobadorId,
        DateTime FechaCreacion
        );
}
