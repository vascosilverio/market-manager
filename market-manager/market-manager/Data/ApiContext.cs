using Microsoft.EntityFrameworkCore;
using market_manager.Models;

namespace market_manager.Data
{
	public class APIContext : DbContext
	{
		public DbSet<Bancas> Bancas { get; set; }
		public DbSet<Reservas> Reservas { get; set; }
		public DbSet<Notificacoes> Notificacoes { get; set; }
		public DbSet<Gestores> Gestores { get; set; }
		public DbSet<Vendedores> Vendedores { get; set; }

		public APIContext(DbContextOptions<APIContext> options)
			:base (options)
		{ 
		
		}
	}
}