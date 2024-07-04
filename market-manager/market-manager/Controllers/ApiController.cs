using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using market_manager.Models;
using market_manager.Data;

namespace market_manager.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ApiController : ControllerBase
	{
		private readonly APIContext _context;

		// Construtor que recebe o contexto da bd APIContext como uma dependência.
		public ApiController(APIContext context)
		{
			// permitir o acesso ao contexto da bd dentro das ações do controlador, _ indica uma variável privada
			_context = context;
		}

		// Create/Edit
		[HttpPost]
		public JsonResult CreateUpdate(Bancas banca)
		{
			if(banca.BancaId == 0)
			{
				_context.Bancas.Add(banca);
			} else
			{
				var BancaNaDb = _context.Bancas.Find(banca.BancaId);

				if (BancaNaDb == null)
					return new JsonResult(NotFound());
				BancaNaDb = banca;
			}

			_context.SaveChanges();

			return new JsonResult(Ok(banca));
		}

		// Read
		[HttpGet]
		public JsonResult Read(int id)
		{
			var result = _context.Bancas.Find(id);
			               
			if(result == null)
				return new JsonResult(NotFound());
			
			return new JsonResult(Ok(result));
		}

		// Delete
		[HttpDelete]
		public JsonResult Delete(int id)
		{
			var result = _context.Bancas.Find(id);

			if(result == null)
				return new JsonResult(NotFound());

			_context.Bancas.Remove(result);
			_context.SaveChanges();

			return new JsonResult(NoContent());
		}

		// Get all
		[HttpGet()]
		public JsonResult GetAll()
		{
			var result = _context.Bancas.ToList();

			return new JsonResult(Ok(result));
		}
	}
}