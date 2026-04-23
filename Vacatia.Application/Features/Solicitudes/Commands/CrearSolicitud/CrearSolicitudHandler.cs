using MediatR;
using Vacatia.Application.Common;
using Vacatia.Application.DTOs;
using Vacatia.Domain.Entities;
using Vacatia.Domain.Enums;
using Vacatia.Domain.Exceptions;
using Vacatia.Domain.Interfaces;
using Vacatia.Domain.Services;
using Vacatia.Domain.ValueObjects;

namespace Vacatia.Application.Features.Solicitudes.Commands.CrearSolicitud
{
    public class CrearSolicitudHandler : IRequestHandler<CrearSolicitudCommand, SolicitudDto>
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ISaldoVacacionesRepository _saldoRepository;
        private readonly SolicitudDomainService _domainService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _uow;

        public CrearSolicitudHandler(ISolicitudRepository solicitudRepository, ISaldoVacacionesRepository saldoRepository, SolicitudDomainService domainService, ICurrentUserService currentUserService, IUnitOfWork uow)
        {
            _solicitudRepository = solicitudRepository;
            _saldoRepository = saldoRepository;
            _domainService = domainService;
            _currentUserService = currentUserService;
            _uow = uow;
        }

        public async Task<SolicitudDto> Handle(CrearSolicitudCommand request, CancellationToken cancellationToken)
        {
            var periodo = new PeriodoFechas(request.FechaInicio, request.FechaFin);

            // Verificar superposicion de fechas
            await _domainService.ValidarSuperposicionAsync(_currentUserService.UserId, periodo, cancellationToken);

            // Verificar si hay saldo de vacaciones suficiente
            if (request.Tipo == TipoSolicitud.Vacaciones)
            {
                var saldoUsuario = await _saldoRepository.ObtenerPorUsuarioAsync(_currentUserService.UserId, _currentUserService.TenantId, request.FechaInicio.Year, cancellationToken);

                if (saldoUsuario == null)
                    throw new DomainException("Este usuario no tiene saldo de vacaciones para el año seleccionado.");

                if (saldoUsuario.DiasDisponibles < periodo.DiasNaturales)
                    throw new DomainException("El usuario no tiene suficientes días de vacaciones disponibles");
            }

            // Crear la solicitud (Factory Method)
            var solicitud = Solicitud.Crear(usuarioId: _currentUserService.UserId,
                usuarioEmail: _currentUserService.Email,
                usuarioNombre: _currentUserService.Nombre,
                tenantId: _currentUserService.TenantId,
                tipo: request.Tipo,
                fechaInicio: request.FechaInicio,
                fechaFin: request.FechaFin,
                motivo: request.Motivo
            );

            //Persistir la solicitud
            await _solicitudRepository.AgregarAsync(solicitud, cancellationToken);
            await _uow.GuardarCambiosAsync(cancellationToken);

            // Mapear a DTO(NUNCA exponer entidades)
            return SolicitudMapper.MapToDto(solicitud);
        }

        
    }
}
