
using Geografia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Geografia.Infrastructure.Data;

public class GeografiaDbContext : DbContext
{
    public GeografiaDbContext(
        DbContextOptions<GeografiaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pais> Paises => Set<Pais>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Ciudad> Ciudades => Set<Ciudad>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.ToTable("Paises", "dbo");

            entity.HasKey(p => p.PaisId);

            entity.Property(p => p.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.CodigoIso2)
                .HasColumnType("char(2)")
                .IsFixedLength()
                .IsUnicode(false);

            entity.Property(p => p.Activo)
                .HasDefaultValue(true);

            entity.Property(p => p.FechaCreacion)
                .HasDefaultValueSql("SYSDATETIME()");

            entity.Property(p => p.FechaModificacion)
                .IsRequired(false);

            entity.HasIndex(p => p.Nombre)
                .IsUnique();

            entity.HasIndex(p => p.CodigoIso2)
                .IsUnique();
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.ToTable("Departamentos", "dbo");

            entity.HasKey(d => d.DepartamentoId);

            entity.Property(d => d.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(d => d.Activo)
                .HasDefaultValue(true);

            entity.Property(d => d.FechaCreacion)
                .HasDefaultValueSql("SYSDATETIME()");

            entity.Property(d => d.FechaModificacion)
                .IsRequired(false);

            entity.HasIndex(d => new { d.PaisId, d.Nombre })
                .IsUnique();

            entity.HasOne(d => d.Pais)
                .WithMany(p => p.Departamentos)
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.ToTable("Ciudades", "dbo");

            entity.HasKey(c => c.CiudadId);

            entity.Property(c => c.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(c => c.Activo)
                .HasDefaultValue(true);

            entity.Property(c => c.FechaCreacion)
                .HasDefaultValueSql("SYSDATETIME()");

            entity.Property(c => c.FechaModificacion)
                .IsRequired(false);

            entity.HasIndex(c => new { c.DepartamentoId, c.Nombre })
                .IsUnique();

            entity.HasOne(c => c.Departamento)
                .WithMany(d => d.Ciudades)
                .HasForeignKey(c => c.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
