using market_manager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace market_manager.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilizadores>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bancas> Bancas { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }

    public class ReservaBanca
    {
        public int BancaId { get; set; }
        public Bancas Banca { get; set; }

        public int ReservaId { get; set; }
        public Reservas Reserva { get; set; }

    }
}
