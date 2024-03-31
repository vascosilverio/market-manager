using market_manager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace market_manager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Bancas> Bancas { get; set; }
        public DbSet<Notificacoes> Notificacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notificacoes>()
                .HasOne(n => n.Destinatario)
                .WithMany()
                .HasForeignKey(n => n.DestinatarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notificacoes>()
                .HasOne(n => n.Registo)
                .WithMany()
                .HasForeignKey(n => n.RegistoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notificacoes>()
                .HasOne(n => n.Reserva)
                .WithMany(r => r.Notificacoes)
                .HasForeignKey(n => n.ReservaId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
