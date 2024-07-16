using market_manager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace market_manager.Data
{
    // Classe que representa o contexto da aplicação com parametrização do tipo IdentityDbContext sendo Utilizadores a classe que representa os utilizadores
    public class ApplicationDbContext : IdentityDbContext<Utilizadores>
    {
        // Construtor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Propriedades que representam as tabelas da base de dados
        public DbSet<Bancas> Bancas { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }

        // Método que é chamado quando o modelo é criado
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }

    // Classe que representa a relação entre as tabelas Bancas e Reservas
    public class ReservaBanca
    {
        public int BancaId { get; set; }
        public Bancas Banca { get; set; }

        public int ReservaId { get; set; }
        public Reservas Reserva { get; set; }

    }
}
