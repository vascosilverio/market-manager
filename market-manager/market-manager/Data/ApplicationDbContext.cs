using market_manager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
                .WithMany(u => u.ListaReservas)
                .HasForeignKey(r => r.VendedorId);

            builder.Entity<Notificacoes>()
                .HasOne(n => n.Vendedor)
                .WithMany(u => u.ListaNotificacoes)
                .HasForeignKey(n => n.VendedorId);

            builder.Entity<Notificacoes>()
                .HasOne(n => n.Gestor)
                .WithMany(u => u.ListaNotificacoes)
                .HasForeignKey(n => n.GestorId);

            builder.Entity<Reservas>()
                .HasMany(r => r.ListaBancas)
                .WithMany(b => b.Reservas)
                .UsingEntity<ReservaBanca>(
                    j => j.HasOne(rb => rb.Banca).WithMany().HasForeignKey(rb => rb.BancaId),
                    j => j.HasOne(rb => rb.Reserva).WithMany().HasForeignKey(rb => rb.ReservaId),
                    j =>
                    {
                        j.Property(rb => rb.BancaId).HasColumnName("BancaId");
                        j.Property(rb => rb.ReservaId).HasColumnName("ReservaId");
                    });
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
