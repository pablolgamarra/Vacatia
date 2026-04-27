using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vacatia.Api.DTOs;
using Vacatia.Application.Features.Solicitudes.Commands.AprobarSolicitud;
using Vacatia.Application.Features.Solicitudes.Commands.CrearSolicitud;
using Vacatia.Application.Features.Solicitudes.Commands.RechazarSolicitud;
using Vacatia.Application.Features.Solicitudes.Queries.ObtenerSolicitudesPropias;

namespace Vacatia.Api.Controllers
{
    /// <summary>
    /// Controller limpio: solo recibe requests, delega a MediatR, devuelve responses.
    /// CERO lógica de negocio aquí.
    /// </summary>
    [ApiController]
    [Route("api/solicitudes")]
    [Authorize] //Requiere JWT en todos los Endpoints
    public class SolicitudesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SolicitudesController(IMediator mediator) {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea una nueva solicitud de vacaciones o permiso.
        /// El usuario se extrae del token JWT (no del body).
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Crear([FromBody] CrearSolicitudRequest request, CancellationToken ct)
        {
            CrearSolicitudCommand command = new(
                request.Tipo,
                request.FechaInicio,
                request.FechaFin,
                request.Motivo
            );

            var resultado = await _mediator.Send(command, ct);

            return CreatedAtAction(nameof(MisSolicitudes), new { }, resultado);
        }

        /// <summary>
        /// Obtiene las solicitudes del usuario autenticado.
        /// </summary>
        [HttpGet("mis-solicitudes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> MisSolicitudes(CancellationToken ct)
        {
            var resultado = await _mediator.Send(new ObtenerSolicitudesPropiasQuery(), ct);
            return Ok(resultado);
        }

        /// <summary>
        /// Aprueba una solicitud. Requiere rol de Aprobador.
        /// </summary>
        [HttpPut("{id:guid}/aprobar")]
        [Authorize(Policy = "EsAprobador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Aprobar(Guid id, CancellationToken ct)
        {
            var resultado = await _mediator.Send(new AprobarSolicitudCommand(id), ct);

            return Ok(resultado);
        }

        /// <summary>
        /// Rechaza una solicitud. Requiere rol de Aprobador.
        /// </summary>

        [HttpPut("{id:Guid}/rechazar")]
        [Authorize(Policy = "EsAprobador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Rechazar(Guid id, [FromBody] RechazarSolicitudRequest request, CancellationToken ct)
        {
            var resultado = await _mediator.Send(new RechazarSolicitudCommand(id, request.MotivoRechazo), ct);

            return Ok(resultado);
        }
    }
}
