using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Vacatia.Application.Common.Interfaces;

namespace Vacatia.Infraestructure.Identity
{
    /// <summary>
    /// Extrae información del usuario desde el token JWT de Azure AD.
    /// Claims estándar de Microsoft Identity Platform:
    ///   - oid  → Object ID (identificador único e inmutable del usuario en AAD)
    ///   - tid  → Tenant ID
    ///   - upn  → User Principal Name (email corporativo)
    ///   - name → Nombre del usuario
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User =>
            _httpContextAccessor.HttpContext?.User ?? 
                throw new InvalidOperationException("No hay contexto HTTP o el usuario no está autenticado.");

        /// <summary>
        /// Object ID de Azure AD — identificador único del usuario.
        /// Usar 'oid' y NO 'sub', ya que 'sub' cambia por aplicación.
        /// </summary>
        public string UserId =>
            User.FindFirst("oid")?.Value
            ?? User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
            ?? throw new InvalidOperationException("Claim 'oid' no encontrado en el token.");
        

        public string Email => 
            User.FindFirst("upn")?.Value 
            ?? User.FindFirst(ClaimTypes.Email)?.Value 
            ?? User.FindFirst("preferred_username")?.Value 
            ?? string.Empty;

        public string Nombre =>
            User.FindFirst("name")?.Value 
            ?? User.FindFirst(ClaimTypes.Name)?.Value 
            ?? string.Empty;

        public string TenantId =>
            User.FindFirst("tid")?.Value
            ?? User.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value
            ?? throw new InvalidOperationException("Claim 'tid' no encontrado en el token.");

        public bool EsAprobador =>
            User.IsInRole("Aprobador") || User.IsInRole("Manager");

        public bool EsAdmin =>
             User.IsInRole("Admin") || User.IsInRole("RRHH");
    }
}
