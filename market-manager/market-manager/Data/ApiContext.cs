using Microsoft.EntityFrameworkCore;
using market_manager.Models;

namespace market_manager.Data
{
	// Define o contexto da bd
	public class APIContext : DbContext
	{
		// Definir as propriedades DbSet para cada tabela da bd
		public DbSet<Bancas> Bancas { get; set; }
		public DbSet<Reservas> Reservas { get; set; }
		public DbSet<Notificacoes> Notificacoes { get; set; }
		public DbSet<Gestores> Gestores { get; set; }
		public DbSet<Vendedores> Vendedores { get; set; }

		// Construtor que recebe as opções de configuração do contexto
		public APIContext(DbContextOptions<APIContext> options)
			:base (options)
		{ 
		
		}
	}
}