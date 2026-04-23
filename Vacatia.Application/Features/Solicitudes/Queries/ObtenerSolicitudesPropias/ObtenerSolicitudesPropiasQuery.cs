using MediatR;
using Vacatia.Application.DTOs;

namespace Vacatia.Application.Features.Solicitudes.Queries.ObtenerSolicitudesPropias
{
    public record ObtenerSolicitudesPropiasQuery() : IRequest<IReadOnlyList<SolicitudDto>>;
}
