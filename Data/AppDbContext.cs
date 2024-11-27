using Conversor_Monedas_Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Conversor_Monedas_Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) // Pasa las opciones al constructor base
        {
        }
        public DbSet<Usuario> Users { get; set; }
        public DbSet<Moneda> Moneda { get; set; }
        public DbSet<Conversion> Conversion { get; set; }
        public DbSet<Suscripcion> Suscripcion { get; set; }
    }
}
