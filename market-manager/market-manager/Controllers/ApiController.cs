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
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace market_manager.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        public SignInManager<Utilizadores> _signInManager;
        private IConfiguration _config;
        private readonly ILogger<ApiController> _logger;

        public ApiController(ApplicationDbContext context,
            SignInManager<Utilizadores> signInManager,
            UserManager<Utilizadores> userManager,
            IConfiguration config,
            ILogger<ApiController> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }

        [HttpPost("registo")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterDTO dto)
        {
            Utilizadores user = new Utilizadores();
            user.UserName = dto.Email;
            user.Email = dto.Email;
            user.NormalizedUserName = user.UserName.ToUpper();
            user.NormalizedEmail = user.Email.ToUpper();
            user.NomeCompleto = dto.NomeCompleto;
            user.DataNascimento = dto.DataNascimento;
            user.Telemovel = dto.Telemovel;
            user.Morada = dto.Morada;
            user.CodigoPostal = dto.CodigoPostal;
            user.Localidade = dto.Localidade;
            user.CC = dto.CC;
            user.NIF = dto.NIF;
            user.Role = "Vendedor";
            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = null;
            user.PasswordHash = new PasswordHasher<Utilizadores>().HashPassword(null, dto.Password);

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

        [HttpPost("login")]
        public async Task<ActionResult> SignInUserAsync([FromBody] LoginDTO dto)
        {
            _logger.LogInformation($"Login attempt for email: {dto.Email}");

            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest("Invalid login data");
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return BadRequest("Invalid email or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (result.Succeeded)
            {
                var token = await GenerateJwtToken(user);
                return Ok(new { Token = token, UserRole = user.Role, UserId = user.Id });
            }

            return BadRequest("Invalid email or password");
        }

        private async Task<string> GenerateJwtToken(Utilizadores user)
        {
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
            {
                _logger.LogError("JWT configuration is missing or invalid");
                throw new InvalidOperationException("JWT configuration is invalid");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(120);

            var token = new JwtSecurityToken(
                jwtIssuer,
                jwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout com sucesso");
        }

        [HttpPost]
        [Authorize(Roles = "Gestor")]
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

            return CreatedAtAction(nameof(ReadBanca), new { id = banca.BancaId }, banca);
        }

        [HttpGet("bancas/{id}")]
        [Authorize]
        public ActionResult<Bancas> ReadBanca(int id)
        {
            var result = _context.Bancas.Find(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("bancas/{id}")]
        [Authorize(Roles = "Gestor")]
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
            banca.FotografiaBanca = "foto.jpg";

            _context.Bancas.Update(banca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("bancas/{id}")]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteBanca(int id)
        {
            var banca = await _context.Bancas.FindAsync(id); 
            if (banca == null)
                return NotFound();

            if (await _context.Reservas.AnyAsync(r => r.ListaBancas.Contains(banca)))
                return Conflict("Cannot delete banca with associated reservas");

            _context.Bancas.Remove(banca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("bancas")]
        [Authorize]
        public async Task<JsonResult> GetAllBancas()
        {
            _logger.LogInformation("GetAllBancas called");
            try
            {
                var user = HttpContext.User;
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("Unauthorized access attempt to GetAllBancas");
                    return new JsonResult(new { success = false, message = "User is not authenticated" }) { StatusCode = 401 };
                }

                _logger.LogInformation($"User {user.Identity.Name} authorized for GetAllBancas");
                var bancas = await _context.Bancas.Include(b => b.Reservas).ToListAsync();

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                return new JsonResult(new { success = true, data = bancas, message = "Bancas retrieved successfully" }, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching bancas");
                return new JsonResult(new { success = false, message = "An error occurred while processing your request." }) { StatusCode = 500 };
            }
        }



        [HttpPost]
        [Authorize]
        public ActionResult<Reservas> CreateReserva([FromBody] ReservasDTO dto)
        {
            Reservas reserva = new Reservas();
            reserva.DataInicio = dto.DataInicio;
            reserva.DataFim = dto.DataFim;
            reserva.UtilizadorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            reserva.EstadoActualReserva = dto.EstadoActualReserva;
            reserva.ListaBancas = _context.Bancas.Where(b => dto.SelectedBancaIds.Contains(b.BancaId)).ToList();
            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ReadReserva), new { id = reserva.ReservaId }, reserva);
        }

        [HttpGet("reservas/{id}")]
        [Authorize]
        public ActionResult<Reservas> ReadReserva(int id)
        {
            var result = _context.Reservas.Include(r => r.ListaBancas).FirstOrDefault(r => r.ReservaId == id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("reservas/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReserva(int id, [FromBody] ReservasDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            var reserva = _context.Reservas.Include(r => r.ListaBancas).FirstOrDefault(r => r.ReservaId == id);
            if (reserva == null)
            {
                return NotFound("Reserva não encontrada");
            }

            if (reserva.UtilizadorId != User.FindFirst(ClaimTypes.NameIdentifier).Value && !User.IsInRole("Gestor"))
                return Forbid();

            reserva.DataInicio = dto.DataInicio;
            reserva.DataFim = dto.DataFim;
            reserva.EstadoActualReserva = User.IsInRole("Gestor") ? dto.EstadoActualReserva : reserva.EstadoActualReserva;
            reserva.ListaBancas = _context.Bancas.Where(b => dto.SelectedBancaIds.Contains(b.BancaId)).ToList();

            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("reservas/{id}")]
        [Authorize]
        public ActionResult<Reservas> DeleteReserva(int id)
        {
            var result = _context.Reservas.Find(id);

            if (result == null)
                return NotFound();

            if (result.UtilizadorId != User.FindFirst(ClaimTypes.NameIdentifier).Value && !User.IsInRole("Gestor"))
                return Forbid();

            _context.Reservas.Remove(result);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        [Route("bancas/{bancaId}/reservas")]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> CreateReservaForBanca(int bancaId, [FromBody] ReservasDTO dto)
        {
            var banca = await _context.Bancas.FindAsync(bancaId);

            if (banca == null)
            {
                return NotFound("Banca não encontrada");
            }

            var reserva = new Reservas
            {
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                UtilizadorId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                EstadoActualReserva = dto.EstadoActualReserva,
                ListaBancas = new List<Bancas> { banca }
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ReadReserva), new { id = reserva.ReservaId }, reserva);
        }

        [HttpGet("reservas")]
        [Authorize]
        public async Task<JsonResult> GetAllReservas()
        {
            _logger.LogInformation("GetAllReservas called");
            try
            {
                var user = HttpContext.User;
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("Unauthorized access attempt to GetAllReservas");
                    return new JsonResult(new { success = false, message = "User is not authenticated" }) { StatusCode = 401 };
                }

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User {userId} authorized for GetAllReservas");

                List<Reservas> reservas;
                if (user.IsInRole("Gestor"))
                {
                    reservas = await _context.Reservas.Include(r => r.ListaBancas).ToListAsync();
                }
                else
                {
                    reservas = await _context.Reservas
                        .Include(r => r.ListaBancas)
                        .Where(r => r.UtilizadorId == userId)
                        .ToListAsync();
                }

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                return new JsonResult(new { success = true, data = reservas, message = "Reservas retrieved successfully" }, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservas");
                return new JsonResult(new { success = false, message = "An error occurred while processing your request." }) { StatusCode = 500 };
            }
        }
    }
}