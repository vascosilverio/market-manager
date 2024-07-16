using market_manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace market_manager.Controllers
{
    [Authorize]
	public class HomeController : Controller
	{
		// Atributo do tipo ILogger, parametrizado com o tipo HomeController
		private readonly ILogger<HomeController> _logger;

		// Construtor
		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		// Retorna a view da página inicial
		public IActionResult Index()
		{
			return View();
		}

		// Retorna a view da página de privacidade
		public IActionResult Privacy()
		{
			return View();
		}

		// Retorna a view de erro
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
