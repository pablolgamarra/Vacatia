using FluentValidation;
using Vacatia.Domain.Enums;

namespace Vacatia.Application.Features.Solicitudes.Commands.CrearSolicitud
{
    public abstract class CrearSolicitudValidator : AbstractValidator<CrearSolicitudCommand>
    {
        public CrearSolicitudValidator()
        {
            RuleFor(x => x.Tipo).IsInEnum().WithMessage("El tipo de solicitud no es válido.");
            RuleFor(x => x.FechaInicio).NotEmpty().WithMessage("La fecha de inicio es requerida.");
            RuleFor(x => x.FechaFin).NotEmpty().GreaterThanOrEqualTo(x => x.FechaInicio).WithMessage("La fecha de fin debe ser igual o mayor a la fecha de inicio.");
            RuleFor(x => x.Motivo).MaximumLength(500).WithMessage("El motivo no puede exceder los 500 caracteres.").When(x => x.Motivo != null);
            RuleFor(x => x.Motivo).NotEmpty().WithMessage("El motivo es obligatorio para solicitudes de permiso.").When(x => x.Tipo != TipoSolicitud.Vacaciones);
        }
    }
}
