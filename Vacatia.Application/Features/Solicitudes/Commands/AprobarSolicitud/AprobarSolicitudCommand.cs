
using MediatR;
using Vacatia.Application.DTOs;

namespace Vacatia.Application.Features.Solicitudes.Commands.AprobarSolicitud
{
    public record AprobarSolicitudCommand(Guid SolicitudId) : IRequest<SolicitudDto>;
}
