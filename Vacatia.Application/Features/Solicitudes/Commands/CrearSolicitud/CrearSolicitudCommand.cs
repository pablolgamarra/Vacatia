
using MediatR;
using Vacatia.Application.DTOs;
using Vacatia.Domain.Enums;

namespace Vacatia.Application.Features.Solicitudes.Commands.CrearSolicitud
{
    public record CrearSolicitudCommand(
        TipoSolicitud Tipo,
        DateOnly FechaInicio,
        DateOnly FechaFin,
        string? Motivo) : IRequest<SolicitudDto>;
}
