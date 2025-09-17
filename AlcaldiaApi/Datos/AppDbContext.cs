
using AlcaldiaApi.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Cargo> Cargos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Cargo>().ToTable("Cargo");
        }
    }
}
