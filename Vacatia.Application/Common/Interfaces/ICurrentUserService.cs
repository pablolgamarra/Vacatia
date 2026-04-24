namespace Vacatia.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; } //Azure AD Object ID
        string Email { get; }
        string Nombre { get; }
        string TenantId { get; }
        bool EsAprobador { get; }
        bool EsAdmin { get; }
    }
}
