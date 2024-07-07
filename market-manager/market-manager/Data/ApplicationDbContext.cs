using market_manager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace market_manager.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bancas> Bancas { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Notificacoes> Notificacoes { get; set; }
        public DbSet<Gestores> Gestores { get; set; }
        public DbSet<Vendedores> Vendedores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
      
            builder.Entity<Reservas>()
                .HasOne(r => r.Vendedor)
                .WithMany(v => v.ListaReservas)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notificacoes>()
                .HasOne(n => n.Vendedor)
                .WithMany(v => v.ListaNotificacoes)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notificacoes>()
                .HasOne(n => n.Gestor)
                .WithMany(g => g.ListaNotificacoes)
                .OnDelete(DeleteBehavior.Cascade);
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
