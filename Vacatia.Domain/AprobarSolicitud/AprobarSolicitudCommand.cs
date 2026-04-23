
using MediatR;
using Vacatia.Application.DTOs;

namespace Vacatia.Domain.AprobarSolicitud
{
    public record AprobarSolicitudCommand(Guid SolicitudId) : IRequest<SolicitudDto>;
}
