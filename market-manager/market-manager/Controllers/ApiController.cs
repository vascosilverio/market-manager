using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using market_manager.Models;
using market_manager.Data;
using Microsoft.AspNetCore.Identity;

namespace market_manager.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ApiController : Controller
	{
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// para fins de autenticação
		/// o _userManager faz a pesquisa nas tabelas da base de dados
		/// </summary>
		private readonly UserManager<IdentityUser> _userManager;

		/// <summary>
		/// para fins de autenticação
		/// o _signInManager é quem gere a autenticação em si, é quem diz ao servidor
		/// que x utilizador da base de dados está agora autenticado, podendo serem assim aceites
		/// requests desse utilizador
		/// </summary>
		public SignInManager<IdentityUser> _signInManager;

		// Construtor que recebe o contexto da bd APIContext como uma dependência.
		public ApiController(ApplicationDbContext context, 
			SignInManager<IdentityUser> signInManager,
			UserManager<IdentityUser> userManager)
		{
			// permitir o acesso ao contexto da bd dentro das ações do controlador, _ indica uma variável privada
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
		}


		/// <summary>
		/// operações da API que
		/// têm a ver com users
		/// </summary>

		[HttpPost]
		[Route("createUser")]
		public async Task<ActionResult> CreateUser() 
		{
			IdentityUser identityUser = new IdentityUser();
			identityUser.UserName = "mmb";
			identityUser.Email = "mmb@teste.com";
			identityUser.NormalizedUserName = identityUser.UserName.ToUpper();
			identityUser.NormalizedEmail = identityUser.Email.ToUpper();
			identityUser.PasswordHash = null;
			identityUser.Id = Guid.NewGuid().ToString();
			identityUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "passteste123");

			var result = await _userManager.CreateAsync(identityUser);
			_context.SaveChanges();

			if (result.Succeeded)
			{
				return Ok("Usuário criado com sucesso");
			}
			else
			{
				return BadRequest("Erro ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));
			}   
		}



		/// <summary>
		/// operações CRUD da API sobre tabelas 
		/// excluindo tudo o que tenha a ver com users
		/// </summary>

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