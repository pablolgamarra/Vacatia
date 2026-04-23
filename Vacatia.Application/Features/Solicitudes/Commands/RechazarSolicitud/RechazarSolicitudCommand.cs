using MediatR;
using Vacatia.Application.DTOs;

namespace Vacatia.Application.Features.Solicitudes.Commands.RechazarSolicitud
{
    public record RechazarSolicitudCommand(Guid SolicitudId, string MotivoRechazo) : IRequest<SolicitudDto>;
}
