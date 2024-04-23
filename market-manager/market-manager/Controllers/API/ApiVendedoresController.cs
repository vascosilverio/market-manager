using market_manager.Data;
using Microsoft.AspNetCore.Mvc;

namespace market_manager.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiVendedoresController : Controller
    {

        public ApplicationDbContext _context;

        public ApiVendedoresController(ApplicationDbContext context) {
            _context = context;
        }
            
        [Route("")]
        public ActionResult Index()
        {
        return Ok(_context.Vendedores.ToList());
        }
    }
}
