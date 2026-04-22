using Vacatia.Domain.Exceptions;

namespace Vacatia.Domain.ValueObjects
{
    // <summary>
    // Value Object que encapsula y valida el periodo para una solicitud
    // </summary>
    public class PeriodoFechas : IEquatable<PeriodoFechas>
    {
        public DateOnly FechaInicio { get; }
        public DateOnly FechaFin { get; }

        public PeriodoFechas (DateOnly fechaInicio, DateOnly fechaFin) 
        {
            if (fechaInicio > fechaFin)
                throw new DomainException("La fecha de inicio debe ser anterior o igual a la fecha de fin");

            if (fechaInicio < DateOnly.FromDateTime(DateTime.UtcNow.Date))
                throw new DomainException("No se pueden crear solicitudes con fechas pasadas");

            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }

        // <summary>Calcula días naturales (inclusivo en ambos extremos).</summary>
        public int DiasNaturales => FechaFin.DayNumber - FechaInicio.DayNumber + 1;

        /// <summary>Verifica si este período se superpone con otro.</summary>
        public bool SeSuperponeCon(PeriodoFechas periodo)
        {
            return FechaInicio <= periodo.FechaFin && FechaFin >= periodo.FechaInicio;
        }

        public bool Equals(PeriodoFechas? other)
        {
            return other != null && FechaInicio == other.FechaInicio && FechaFin == other.FechaFin;
        }

        public override bool Equals(object? obj) => Equals(obj as PeriodoFechas);
        public override int GetHashCode() => HashCode.Combine(FechaInicio, FechaFin);
    }
}
