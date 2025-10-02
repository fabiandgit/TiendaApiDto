using Microsoft.EntityFrameworkCore;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Data
{
    public class TiendaApiContext : DbContext
    {
        public TiendaApiContext(DbContextOptions<TiendaApiContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Venta> Ventas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relación: Producto → Ventas
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Producto)
                .WithMany(p => p.Ventas) // propiedad inversa en Producto
                .HasForeignKey(v => v.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de relación: Empleado → Ventas
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Empleado)
                .WithMany(e => e.Ventas) // propiedad inversa en Empleado
                .HasForeignKey(v => v.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de precisión para Precio
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}