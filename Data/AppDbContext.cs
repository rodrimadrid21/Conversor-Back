using Conversor_Monedas_Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conversor_Monedas_Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) //constructor. options = config
            : base(options) // Pasa la config al constructor base (dbcontext)
        {
        }
        public DbSet<Usuario> Users { get; set; }
        public DbSet<Moneda> Moneda { get; set; }
        public DbSet<Conversion> Conversion { get; set; }
        public DbSet<Suscripcion> Suscripcion { get; set; }
    }
}
