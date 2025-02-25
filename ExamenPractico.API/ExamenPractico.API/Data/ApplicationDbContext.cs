using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExamenPractico.API.Models;

namespace ExamenPractico.API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para la entidad Empleado
        public DbSet<Empleado> Empleados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar explícitamente el tipo decimal para Salario
            modelBuilder.Entity<Empleado>()
                .Property(e => e.Salario)
                .HasColumnType("decimal(18,2)");
        }
    }
}
