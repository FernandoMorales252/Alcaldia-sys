
using AlcaldiaApi.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Definicion de los DbSet para las entidades 
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<TipoDocumento> Tipos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configuracion de las tablas y relaciones si es necesario
            modelBuilder.Entity<Cargo>().ToTable("Cargo");
            modelBuilder.Entity<Municipio>().ToTable("Municipio");
            modelBuilder.Entity<TipoDocumento>().ToTable("TipoDocumento");
        }
    }
}
