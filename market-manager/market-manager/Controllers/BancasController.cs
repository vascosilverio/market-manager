using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using market_manager.Data;
using market_manager.Models;
using Microsoft.AspNetCore.Authorization;

namespace market_manager.Controllers
{
    public class BancasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BancasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bancas
        //mostra todas as bancas da base de dados
        [AllowAnonymous] //permite a visualização das bancas da base de dados, sem a necessidade de autenticação
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bancas.ToListAsync());
        }

        // GET: Bancas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bancas = await _context.Bancas
                .FirstOrDefaultAsync(m => m.BancaId == id);
            if (bancas == null)
            {
                return NotFound();
            }

            return View(bancas);
        }

        // GET: Bancas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bancas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeIdentificadorBanca,CategoriaBanca,LarguraAux,ComprimentoAux,LocalizacaoX,LocalizacaoY,EstadoActualBanca")] Bancas banca)
        {

            if (ModelState.IsValid)
            {
                //transferir o valor de PropinasAux para Propinas
                banca.Largura=Convert.ToDecimal(banca.LarguraAux.Replace('.',','));
                banca.Comprimento = Convert.ToDecimal(banca.ComprimentoAux.Replace('.', ','));
                _context.Add(banca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banca);
        }

        // GET: Bancas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bancas = await _context.Bancas.FindAsync(id);
            if (bancas == null)
            {
                return NotFound();
            }
            return View(bancas);
        }

        // POST: Bancas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BancaId,NomeIdentificadorBanca,CategoriaBanca,Largura,Comprimento,LocalizacaoX,LocalizacaoY,EstadoActualBanca")] Bancas bancas)
        {
            if (id != bancas.BancaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bancas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BancasExists(bancas.BancaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bancas);
        }

        // GET: Bancas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bancas = await _context.Bancas
                .FirstOrDefaultAsync(m => m.BancaId == id);
            if (bancas == null)
            {
                return NotFound();
            }

            return View(bancas);
        }

        // POST: Bancas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bancas = await _context.Bancas.FindAsync(id);
            if (bancas != null)
            {
                _context.Bancas.Remove(bancas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BancasExists(int id)
        {
            return _context.Bancas.Any(e => e.BancaId == id);
        }
    }
}
