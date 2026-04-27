using Vacatia.Domain.Enums;

namespace Vacatia.Api.DTOs
{
    public record CrearSolicitudRequest
    (
        TipoSolicitud Tipo,
        DateOnly FechaInicio,
        DateOnly FechaFin,
        string? Motivo
    );
}
