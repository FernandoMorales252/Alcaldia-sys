
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
        public DbSet<Proyecto> Proyectos { get; set; }

        public DbSet<Empleado> Empleados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configuracion de las tablas y relaciones si es necesario
            modelBuilder.Entity<Cargo>().ToTable("Cargo");
            modelBuilder.Entity<Municipio>().ToTable("Municipio");
            modelBuilder.Entity<TipoDocumento>().ToTable("TipoDocumento");

            modelBuilder.Entity<Empleado>().ToTable("Empleado");

            // Configuracion de las relaciones para la entidad Documento
            modelBuilder.Entity<Documento>()
                .HasOne(d => d.TipoDocumento) // Un documento tiene un TipoDocumento
                .WithMany(td => td.Documentos) // Un TipoDocumento tiene muchos documentos
                .HasForeignKey(d => d.TipoDocumentoId); // La clave foránea es TipoDocumentoId

            modelBuilder.Entity<Documento>()
                .HasOne(d => d.Municipio) // Un documento tiene un Municipio
                .WithMany(m => m.Documentos) // Un Municipio tiene muchos documentos
                .HasForeignKey(d => d.MunicipioId); // La clave foránea es MunicipioId


            // Configuracion de las relaciones para la entidad Empleado
            //Relacion entre Empleado y Cargo
            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Cargo)
                .WithMany(c => c.Empleados)
                .HasForeignKey(e => e.CargoId);
            //Relacion entre Empleado y Municipio
            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Municipio)
                .WithMany(m => m.Empleados)
                .HasForeignKey(e => e.MunicipioId);

            modelBuilder.Entity<Proyecto>()
             .HasOne(d => d.Municipio) // Un documento tiene un Municipio
             .WithMany(m => m.Proyectos) // Un Municipio tiene muchos documentos
             .HasForeignKey(d => d.MunicipioId); // La clave foránea es MunicipioId
        }
    }
}
