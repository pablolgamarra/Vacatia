using Vacatia.Domain.Enums;
using Vacatia.Domain.Exceptions;
using Vacatia.Domain.ValueObjects;

namespace Vacatia.Domain.Entities
{
    //<summary>
    //Aggregate Root: Solicitud de vacaciones o permiso
    //</summary>
    public class Solicitud
    {
        public Guid Id { get; private set; }
        public string UsuarioId { get; private set; }
        public string UsuarioEmail { get; private set; }
        public string UsuarioNombre { get; private set; }
        public string UsuarioNombreMostrar { get; private set; }

        public TipoSolicitud Tipo { get; private set; }
        public PeriodoFechas Periodo { get; private set; }
        public EstadoSolicitud Estado { get; private set; }

        public string? Motivo { get; private set; }
        public string? MotivoRechazo { get; private set; }
        public string AprobadorId { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime FechaActualizacion { get; private set; }

        // Futuro multi-tenant
        public string TenantId { get; private set; }

        private Solicitud() { }

        // Factory Method (encapsulamiento de creación válida)
        public static Solicitud Crear(string usuarioId, string usuarioEmail, string usuarioNombre, string tenantId, TipoSolicitud tipo, DateOnly fechaInicio, DateOnly fechaFin, string? motivo = null) 
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new DomainException("El ID de usuario es requerido.");

            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new DomainException("El ID de usuario es requerido.");

            var periodo = new PeriodoFechas(fechaInicio, fechaFin); // validacion interna

            return new Solicitud
            {
                Id = Guid.NewGuid(), UsuarioId = usuarioId, UsuarioEmail = usuarioEmail, UsuarioNombre = usuarioNombre, TenantId = tenantId, Tipo = tipo, Periodo = periodo, Estado = EstadoSolicitud.Pendiente, Motivo = motivo, FechaCreacion = DateTime.UtcNow, FechaActualizacion = DateTime.UtcNow
            };
        }

        // Comportamientos
        public void Aprobar(string aprobadorId)
        {
            if (!Estado.TransicionValidaA(EstadoSolicitud.Aprobado))
                throw new DomainException($"No se puede aprobar una solicitud en estado '{Estado}'");

            if (string.IsNullOrEmpty(aprobadorId))
                throw new DomainException("El ID del aprobador es obligatorio");

            Estado = EstadoSolicitud.Aprobado;
            AprobadorId = aprobadorId;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void Rechazar(string aprobadorId, string motivoRechazo)
        {
            if (!Estado.TransicionValidaA(EstadoSolicitud.Rechazado))
                throw new DomainException($"No se puede rechazar una solicitud en estado '{Estado}'");

            if (string.IsNullOrEmpty(motivoRechazo))
                throw new DomainException("El motivo de rechazo es obligatorio");

            Estado = EstadoSolicitud.Rechazado;
            AprobadorId = motivoRechazo;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void Cancelar()
        {
            if (!Estado.TransicionValidaA(EstadoSolicitud.Cancelado))
                throw new DomainException($"No se puede cancelar una solicitud en estado '{Estado}'");
        }

        /// <summary>Expone los días que consume esta solicitud.</summary>
        public int DiasConsumidos => Periodo.DiasNaturales;
    }
}
