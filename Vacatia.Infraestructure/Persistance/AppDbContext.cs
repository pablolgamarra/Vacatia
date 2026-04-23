using Microsoft.EntityFrameworkCore;
using Vacatia.Domain.Entities;
using Vacatia.Domain.ValueObjects;

namespace Vacatia.Infraestructure.Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Solicitud> Solicitudes { get; set; } = null!;
        public DbSet<SaldoVacaciones> SaldosVacaciones { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Solicitud ──────────────────────────────────────────────
            modelBuilder.Entity<Solicitud>(e =>
            {
                e.ToTable("solicitudes");
                e.HasKey(x => x.Id);

                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.UsuarioId).HasColumnName("usuario_id").IsRequired().HasMaxLength(100);
                e.Property(x => x.UsuarioEmail).HasColumnName("usuario_email").IsRequired().HasMaxLength(256);
                e.Property(x => x.UsuarioNombre).HasColumnName("usuario_nombre").IsRequired().HasMaxLength(256);
                e.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired().HasMaxLength(100);
                e.Property(x => x.Tipo).HasColumnName("tipo").HasConversion<string>();
                e.Property(x => x.Motivo).HasColumnName("motivo").HasMaxLength(500);
                e.Property(x => x.MotivoRechazo).HasColumnName("motivo_rechazo").HasMaxLength(500);
                e.Property(x => x.AprobadorId).HasColumnName("aprobador_id").HasMaxLength(100);
                e.Property(x => x.FechaCreacion).HasColumnName("fecha_creacion");
                e.Property(x => x.FechaActualizacion).HasColumnName("fecha_actualizacion");

                // Value Object: EstadoSolicitud → almacenado como string
                e.Property(x => x.Estado)
                 .HasColumnName("estado")
                 .HasConversion(
                     v => v.Valor,
                     v => EstadoSolicitud.Desde(v))
                 .HasMaxLength(20);

                // Value Object: PeriodoFechas → columnas owned
                e.OwnsOne(x => x.Periodo, p =>
                {
                    p.Property(x => x.FechaInicio).HasColumnName("fecha_inicio").IsRequired();
                    p.Property(x => x.FechaFin).HasColumnName("fecha_fin").IsRequired();
                });

                // Índices para queries frecuentes
                e.HasIndex(x => x.UsuarioId).HasDatabaseName("ix_solicitudes_usuario_id");
                e.HasIndex(x => x.TenantId).HasDatabaseName("ix_solicitudes_tenant_id");
            });

            // ── SaldoVacaciones ────────────────────────────────────────
            modelBuilder.Entity<SaldoVacaciones>(e =>
            {
                e.ToTable("saldos_vacaciones");
                e.HasKey(x => x.Id);

                e.Property(x => x.UsuarioId).HasColumnName("usuario_id").IsRequired().HasMaxLength(100);
                e.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired().HasMaxLength(100);
                e.Property(x => x.DiasDisponibles).HasColumnName("dias_disponibles");
                e.Property(x => x.DiasUsados).HasColumnName("dias_usados");
                e.Property(x => x.Anio).HasColumnName("anio");

                e.HasIndex(x => new { x.UsuarioId, x.TenantId, x.Anio })
                 .IsUnique()
                 .HasDatabaseName("ix_saldos_usuario_tenant_anio");
            });
        }
    }
}
