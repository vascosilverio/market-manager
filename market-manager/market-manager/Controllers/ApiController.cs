using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using market_manager.Models;
using market_manager.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using market_manager.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace market_manager.Controllers
{
	[Route("api/[action]")]
	[ApiController]
	public class ApiController : Controller
	{
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// para fins de autenticação
		/// o _userManager faz a pesquisa nas tabelas da base de dados
		/// </summary>
		private readonly UserManager<Utilizadores> _userManager;

		/// <summary>
		/// para fins de autenticação
		/// o _signInManager é quem gere a autenticação em si, é quem diz ao servidor
		/// que x utilizador da base de dados está agora autenticado, podendo serem assim aceites
		/// requests desse utilizador
		/// </summary>
		public SignInManager<Utilizadores> _signInManager;

		// 
		private IConfiguration _config;

		// Construtor que recebe o contexto da bd APIContext como uma dependência.
		public ApiController(ApplicationDbContext context,
			SignInManager<Utilizadores> signInManager,
			UserManager<Utilizadores> userManager,
			IConfiguration config)
		{
			// permitir o acesso ao contexto da bd dentro das ações do controlador, _ indica uma variável privada
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
			_config = config;
		}

		
		

		/// <summary>
		/// operações da API que
		/// têm a ver com users
		/// </summary>

		// criar user
		[HttpPost]
		public async Task<ActionResult> CreateUser([FromQuery] string email, [FromQuery] string password)
		{
			Utilizadores user = new Utilizadores();
			user.UserName = email;
			user.Email = email;
			user.NormalizedUserName = user.UserName.ToUpper();
			user.NormalizedEmail = user.Email.ToUpper();
			user.NomeCompleto = email;
			user.PasswordHash = null;
			user.Role = "Vendedor";
			user.Id = Guid.NewGuid().ToString();
			user.PasswordHash = new PasswordHasher<Utilizadores>().HashPassword(null, password);

			var result = await _userManager.CreateAsync(user);
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


		// autenticar user
		[HttpGet]
		// necessario o email e a password
		public async Task<ActionResult> SignInUserAsync([FromQuery] string email, [FromQuery] string password)
		{

			Utilizadores user = _userManager.FindByEmailAsync(email).Result; // .Result pois é um método assíncrono
																			 // o resultado da tarefa acima vai ser um IdentityUser

			// se o user existir
			if (user != null)
			{
				/// <summary>
				/// Verificação da password
				/// Recebe utilizador que esta na bd e recebe a password
				/// vai fazer o hash da password, se for igual ao da bd, houve sucesso
				/// </summary>
				PasswordVerificationResult passWorks = new PasswordHasher<Utilizadores>().VerifyHashedPassword(null, user.PasswordHash, password);
				if (passWorks.Equals(PasswordVerificationResult.Success))
				{
					// aqui é adicionado à sessão o cookie de autenticação que irá permitir que o user autenticado faça requests
					var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
					var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

					var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
					  _config["Jwt:Issuer"],
					  null,
					  expires: DateTime.Now.AddMinutes(120),
					  signingCredentials: credentials);

					var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

					return Ok(token);
				}


			}
			return BadRequest("Erro ao autenticar usuário");

		}


		// logout user
		[HttpPost]
		public async Task<ActionResult> LogoutUser()
		{
			// aqui é removido da sessão o cookie de autenticação, revogando as autorizações de requests ao utilizador atual
			await _signInManager.SignOutAsync();
			return Ok("Logout com sucesso");
		}


		/// <summary>
		/// operações CRUD da API sobre bancas
		/// </summary>

		// Create/Edit
		//[HttpPost]
		//public JsonResult CreateUpdate(Bancas banca)
		//{
			//if (banca.BancaId == 0)
			//{
				//_context.Bancas.Add(banca);
			//}
			//else
			//{
				//var BancaNaDb = _context.Bancas.Find(banca.BancaId);

				//if (BancaNaDb == null)
					//return new JsonResult(NotFound());
				//BancaNaDb = banca;
			//}

			//_context.SaveChanges();

			//return new JsonResult(Ok(banca));
		//}

		// Create
		[HttpPost]
		public ActionResult<Bancas> CreateBanca([FromBody] BancasDTO dto)
		{
			Bancas banca = new Bancas();
			banca.NomeIdentificadorBanca = dto.NomeIdentificadorBanca;
			banca.CategoriaBanca = dto.CategoriaBanca;
			banca.Largura = dto.Largura;
			banca.Comprimento = dto.Comprimento;
			banca.LocalizacaoX = dto.LocalizacaoX;
			banca.LocalizacaoY = dto.LocalizacaoY;
			banca.EstadoAtualBanca = dto.EstadoAtualBanca;
			banca.FotografiaBanca = dto.FotografiaBanca;

			_context.Bancas.Add(banca);
			_context.SaveChanges();

			return Ok("Banca criada com sucesso");
		}

		// Read
		[HttpGet]
		public JsonResult ReadBanca(int id)
		{
			var result = _context.Bancas.Find(id);

			if (result == null)
				return new JsonResult(NotFound());

			return new JsonResult(Ok(result));
		}

		// Update
		[HttpPut]
		public async Task<IActionResult> UpdateBanca(int id, [FromBody] BancasDTO dto)
		{
			if (dto == null)
			{
				return BadRequest("Invalid data.");
			}

			var banca = _context.Bancas.Find(id);
			if (banca == null)
			{
				return NotFound("Banca não encontrada");
			}

			banca.NomeIdentificadorBanca = dto.NomeIdentificadorBanca;
			banca.CategoriaBanca = dto.CategoriaBanca;
			banca.Largura = dto.Largura;
			banca.Comprimento = dto.Comprimento;
			banca.LocalizacaoX = dto.LocalizacaoX;
			banca.LocalizacaoY = dto.LocalizacaoY;
			banca.EstadoAtualBanca = dto.EstadoAtualBanca;
			banca.FotografiaBanca = dto.FotografiaBanca;

			_context.Bancas.Update(banca);
			await _context.SaveChangesAsync();

			return Ok("Estado da Banca Atualizado com sucesso");
		}

		// Delete
		[HttpDelete]
		public JsonResult DeleteBanca(int id)
		{
			var result = _context.Bancas.Find(id);

			if (result == null)
				return new JsonResult(NotFound());

			_context.Bancas.Remove(result);
			_context.SaveChanges();

			return new JsonResult(NoContent());
		}

		// Get all
		[HttpGet()]
		public JsonResult GetAllBancas()
		{
			var result = _context.Bancas.ToList();

			return new JsonResult(Ok(result));
		}


		/// <summary>
		/// operações CRUD da API sobre reservas
		/// </summary>

		// Create
		[HttpPost]
		public ActionResult<Reservas> CreateReserva([FromBody] ReservasDTO dto)
		{
			Reservas reserva = new Reservas();
			reserva.DataInicio = dto.DataInicio;
			reserva.DataFim = dto.DataFim;
			reserva.UtilizadorId = dto.UtilizadorId;
			reserva.EstadoActualReserva = dto.EstadoActualReserva;
			_context.Reservas.Add(reserva);
			_context.SaveChanges();

			return Ok("Reserva criada com sucesso");
		}

		// Read
		[HttpGet]
		public JsonResult ReadReserva(int id)
		{
			var result = _context.Reservas.Find(id);

			if (result == null)
				return new JsonResult(NotFound());

			return new JsonResult(Ok(result));
		}

		// Update
		[HttpPut]
		public async Task<IActionResult> UpdateReservaEstado(int id, [FromBody] ReservasDTO dto)
		{
			if (dto == null)
			{
				return BadRequest("Invalid data.");
			}

			var reserva = _context.Reservas.Find(id);
			if (reserva == null)
			{
				return NotFound("Reserva não encontrada");
			}

			reserva.DataInicio = dto.DataInicio;
			reserva.DataFim = dto.DataFim;
			reserva.UtilizadorId = dto.UtilizadorId;
			reserva.EstadoActualReserva = dto.EstadoActualReserva;

			_context.Reservas.Update(reserva);
			await _context.SaveChangesAsync();

			return Ok("Estado da Reserva Atualizado com sucesso");
		}

		// Delete
		[HttpDelete]
		public JsonResult DeleteReserva(int id)
		{
			var result = _context.Reservas.Find(id);

			if (result == null)
				return new JsonResult(NotFound());

			_context.Reservas.Remove(result);
			_context.SaveChanges();

			return new JsonResult(NoContent());
		}

		// Get all
		[HttpGet()]
		public JsonResult GetAllReservas()
		{
			var result = _context.Reservas.ToList();

			return new JsonResult(Ok(result));
		}
	}
}