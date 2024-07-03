using Microsoft.EntityFrameworkCore;
using market_manager.Models;

namespace market_manager.Data
{
	public class APIContext : DbContext
	{

		public APIContext(DbContextOptions<APIContext> options)
			:base (options)
		{ 
		
		}
	}
}
