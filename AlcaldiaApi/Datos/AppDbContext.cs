
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
        public DbSet<Documento> Documentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configuracion de las tablas y relaciones si es necesario
            modelBuilder.Entity<Cargo>().ToTable("Cargo");
            modelBuilder.Entity<Municipio>().ToTable("Municipio");
            modelBuilder.Entity<TipoDocumento>().ToTable("TipoDocumento");

            // Configuracion de las relaciones para la entidad Documento
            modelBuilder.Entity<Documento>()
                .HasOne(d => d.TipoDocumento) // Un documento tiene un TipoDocumento
                .WithMany(td => td.Documentos) // Un TipoDocumento tiene muchos documentos
                .HasForeignKey(d => d.TipoDocumentoId); // La clave foránea es TipoDocumentoId

            modelBuilder.Entity<Documento>()
                .HasOne(d => d.Municipio) // Un documento tiene un Municipio
                .WithMany(m => m.Documentos) // Un Municipio tiene muchos documentos
                .HasForeignKey(d => d.MunicipioId); // La clave foránea es MunicipioId
        }
    }
}
