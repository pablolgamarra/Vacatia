using Vacatia.Domain.Exceptions;

namespace Vacatia.Domain.Entities
{
    public class SaldoVacaciones
    {
        public Guid Id { get; private set; }
        public string UsuarioId { get; private set; }
        public string TenantId { get; private set; }
        public decimal DiasDisponibles { get; private set; }
        public decimal DiasUsados { get; private set; }
        public int Anio { get; private set; }

        private SaldoVacaciones() { }

        public static SaldoVacaciones Crear(string usuarioId, string tenantId, decimal diasDisponibles, int anio)
        {
            if (diasDisponibles < 0)
                throw new DomainException("Los días disponibles no pueden ser negativos.");

            return new SaldoVacaciones
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                TenantId = tenantId,
                DiasDisponibles = diasDisponibles,
                DiasUsados = 0,
                Anio = anio
            };
        }

        public void ConsumirDias(decimal dias)
        {
            if (dias <= 0)
                throw new DomainException("Los días a consumir deben ser positivos.");

            if (dias > DiasDisponibles)
                throw new DomainException($"No hay días disponibles para utilizar. Disponibles: {DiasDisponibles}. Días Solicitados: {dias}");

            DiasDisponibles -= dias;
            DiasUsados += dias;
        }

        public void RestaurarDias(decimal dias)
        {
            if (dias <= 0)
                throw new DomainException("Los días a restaurar deben ser positivos.");

            DiasDisponibles += dias;
            DiasUsados -= Math.Max(0, DiasUsados - dias);
        }
    }
}
