using MediatR;
using Vacatia.Application.Common;
using Vacatia.Application.DTOs;
using Vacatia.Domain.Interfaces;

namespace Vacatia.Application.Features.Solicitudes.Queries.ObtenerSolicitudesPropias
{
    public class ObtenerSolicitudesPropiasHandler : IRequestHandler<ObtenerSolicitudesPropiasQuery, IReadOnlyList<SolicitudDto>>
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ICurrentUserService _currentUserService;

        public ObtenerSolicitudesPropiasHandler(ISolicitudRepository solicitudRepository, ICurrentUserService currentUserService)
        {
            _solicitudRepository = solicitudRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IReadOnlyList<SolicitudDto>> Handle(ObtenerSolicitudesPropiasQuery request, CancellationToken cancellationToken)
        {
            var solicitudes = await _solicitudRepository.ObtenerPorUsuarioAsync(_currentUserService.UserId, _currentUserService.TenantId, cancellationToken);

            return solicitudes.Select(s => SolicitudMapper.MapToDto(s)).ToList();
        }
    }
}
