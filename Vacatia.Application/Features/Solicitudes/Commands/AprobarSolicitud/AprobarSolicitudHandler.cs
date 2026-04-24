using MediatR;
using Vacatia.Application.Common.Interfaces;
using Vacatia.Application.DTOs;
using Vacatia.Domain.Enums;
using Vacatia.Domain.Exceptions;
using Vacatia.Domain.Interfaces;

namespace Vacatia.Application.Features.Solicitudes.Commands.AprobarSolicitud
{
    public class AprobarSolicitudHandler : IRequestHandler<AprobarSolicitudCommand, SolicitudDto>
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ISaldoVacacionesRepository _saldoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _uow;

        public AprobarSolicitudHandler(ISolicitudRepository solicitudRepository, ISaldoVacacionesRepository saldoRepository, ICurrentUserService currentUserService, IUnitOfWork uow)
        {
            _solicitudRepository = solicitudRepository;
            _saldoRepository = saldoRepository;
            _currentUserService = currentUserService;
            _uow = uow;
        }

        public async Task<SolicitudDto> Handle(AprobarSolicitudCommand request, CancellationToken cancellationToken)
        {
            var solicitud = await _solicitudRepository.ObtenerPorIdAsync(request.SolicitudId, cancellationToken);

            if (solicitud == null)
                throw new DomainException("Solicitud no encontrada");

            // Verificar que usuario no se apruebe a sí mismo
            if (solicitud.UsuarioId == _currentUserService.UserId)
                throw new DomainException("Un usuario no se puede aprobar su propia solicitud");

            solicitud.Aprobar(_currentUserService.UserId);

            // Actualizar el saldo de vacaciones del usuario
            if(solicitud.Tipo == TipoSolicitud.Vacaciones)
            {
                var saldo = await _saldoRepository.ObtenerPorUsuarioAsync(solicitud.UsuarioId, solicitud.TenantId, solicitud.Periodo.FechaInicio.Year, cancellationToken);

                if (saldo == null)
                    throw new DomainException("Saldo de vacaciones no encontrado");

                saldo.ConsumirDias(solicitud.DiasConsumidos);
                _saldoRepository.Actualizar(saldo);
            }

            _solicitudRepository.Actualizar(solicitud);
            await _uow.GuardarCambiosAsync(cancellationToken);

            return SolicitudMapper.MapToDto(solicitud);
        }
    }
}
