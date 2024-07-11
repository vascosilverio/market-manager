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
        /*
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
         var listaDb = _context.Vendedores.ToList();
            var listaRes = new List<VendedorDTO>();

            foreach (var item in listaDb) {
                VendedorDTO coldto = new VendedorDTO();
                coldto.Nome = item.PrimeiroNome;
                coldto.CC = item.CC;

            }

            return Ok(listaRes);
        }
        
       [HttpPost]
       [Route("")]
       public ActionResult InsereVendedor([FromBody] VendedorDTO vendedor) {
           VendedorDTO vendedorCol = new VendedoresDTO;
           vendedorCol.nome = 

       }
       */

    }
}
