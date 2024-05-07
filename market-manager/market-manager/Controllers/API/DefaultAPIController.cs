using market_manager.Data;
using market_manager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
    
namespace market_manager.Controllers.API
        
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultAPIController : ControllerBase
    {
        public ApplicationDbContext _context;

        public UserManager<IdentityUser> _userManager;

        public SignInManager<IdentityUser> _signInManager;

        public DefaultAPIController(ApplicationDbContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return Ok("Hello");
        }

        [HttpGet]
        [Route("createUser")]
        public ActionResult CreateUser()
        {
            try
            {
                IdentityUser user = new IdentityUser();
                
                user.UserName = "aluno22350@ipt.pt";
                user.Email = "aluno22350@ipt.pt";
                user.Id = Guid.NewGuid().ToString();
                user.EmailConfirmed = true;
                user.PasswordHash = null;
                user.NormalizedUserName = user.UserName.ToUpper();
                user.NormalizedEmail = user.Email.ToUpper();
                user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "qsc23FTP%/");
                
                _userManager.CreateAsync(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocorreu um erro ao criar o utilizador: " + ex.Message);
                throw;
            }
            

            return Ok("Hello");
        }

        /*
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            var listaDb = _context.Reservas.ToList();
            var listaRes = new List<ColecaoDTO>();

            foreach (var colecao in listaDb)
            {
                ColecaoDTO coldto = new ColecaoDTO();
                coldto.Descricao = colecao.Descricao;
                coldto.Nome = colecao.Nome;

                listaRes.Add(coldto);
            }

            return Ok(listaRes);
        }

        [HttpPost]
        [Route("insereColecao")]
        public ActionResult InsereColecao([FromBody] ColecaoDTO dto)
        {
            Colecao colecao = new Colecao();
            colecao.Nome = dto.Nome;
            colecao.Descricao = dto.Descricao;

            _context.Colecoes.Add(colecao);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("editColecao")]
        public ActionResult EditColecao([FromBody] ColecaoDTO dto, [FromQuery] int id)
        {
            Colecao colecao = _context.Colecoes.Where(c => c.Id == id).FirstOrDefault();

            colecao.Nome = dto.Nome;
            colecao.Descricao = dto.Descricao;

            _context.Colecoes.Update(colecao);
            _context.SaveChanges();

            return Ok();
        }
        */

    }
}

