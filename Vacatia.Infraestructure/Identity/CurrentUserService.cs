using Microsoft.AspNetCore.Http;
using Vacatia.Application.Common;

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

        public string UserId => throw new NotImplementedException();

        public string Email => throw new NotImplementedException();

        public string Nombre => throw new NotImplementedException();

        public string TenantId => throw new NotImplementedException();

        public bool EsAprobador => throw new NotImplementedException();

        public bool EsAdmin => throw new NotImplementedException();
    }
}
