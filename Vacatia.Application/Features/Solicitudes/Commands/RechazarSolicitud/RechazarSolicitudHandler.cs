using MediatR;
using Vacatia.Application.Common.Interfaces;
using Vacatia.Application.DTOs;
using Vacatia.Domain.Exceptions;
using Vacatia.Domain.Interfaces;

namespace Vacatia.Application.Features.Solicitudes.Commands.RechazarSolicitud
{
    public class RechazarSolicitudHandler : IRequestHandler<RechazarSolicitudCommand, SolicitudDto>
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ISaldoVacacionesRepository _saldoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _uow;

        public RechazarSolicitudHandler(
            ISolicitudRepository solicitudRepository,
            ISaldoVacacionesRepository saldoRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork uow)
        {
            _solicitudRepository = solicitudRepository;
            _saldoRepository = saldoRepository;
            _currentUserService = currentUserService;
            _uow = uow;
        }

        public async Task<SolicitudDto> Handle(RechazarSolicitudCommand request, CancellationToken cancellationToken)
        {
            var solicitud = await _solicitudRepository.ObtenerPorIdAsync(request.SolicitudId, cancellationToken);

            if (solicitud == null)
                throw new DomainException("Solicitud no encontrada");

            if (solicitud.UsuarioId == _currentUserService.UserId)
                throw new DomainException("Un usuario no se puede aprobar su propia solicitud");

            solicitud.Rechazar(_currentUserService.UserId, request.MotivoRechazo);

            _solicitudRepository.Actualizar(solicitud);
            await _uow.GuardarCambiosAsync(cancellationToken);

            return SolicitudMapper.MapToDto(solicitud);
        }
    }
}
