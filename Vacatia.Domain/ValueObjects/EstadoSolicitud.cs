namespace Vacatia.Domain.ValueObjects
{
    /// <summary>
    /// Value Object que representa el estado de una solicitud.
    /// Encapsula las transiciones válidas como regla de dominio.
    /// </summary>
    public class EstadoSolicitud : IEquatable<EstadoSolicitud>
    {
        public static readonly EstadoSolicitud Pendiente = new("Pendiente");
        public static readonly EstadoSolicitud Aprobado = new("Aprobado");
        public static readonly EstadoSolicitud Rechazado = new("Rechazado");
        public static readonly EstadoSolicitud Cancelado = new("Cancelado");

        public static readonly Dictionary<string, EstadoSolicitud[]> _transicionesValidas = new()
        {
            [Pendiente.Valor] = [Aprobado, Rechazado, Cancelado],
            [Aprobado.Valor] = [Cancelado],
            [Rechazado.Valor] = [],
            [Cancelado.Valor] = [],
        };

        public string Valor { get; }

        public EstadoSolicitud (string valor)
        {
            if (string.IsNullOrEmpty(valor)) throw new ArgumentException("Valor no específicado");
            Valor = valor;
        }

        public bool TransicionValidaA (EstadoSolicitud nuevoValor)
        {
            return _transicionesValidas[Valor].Contains(nuevoValor);
        }

        public static EstadoSolicitud Desde(string valor) =>
            valor switch
            {
                "Pendiente" => Pendiente,
                "Aprobado" => Aprobado,
                "Rechazado" => Rechazado,
                "Cancelado" => Cancelado,
                _ => throw new ArgumentException($"El estado {valor} no es válido")
            };

        public bool Equals(EstadoSolicitud? other)
        {
            throw new NotImplementedException();
        }
        public override bool Equals(object? obj) => Equals(obj as EstadoSolicitud);
        public override int GetHashCode() => Valor.GetHashCode();
        public override string ToString() => Valor;
    }
}
