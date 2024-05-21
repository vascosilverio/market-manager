
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using market_manager.Data;
using market_manager.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace market_manager.Controllers
{
    
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        //mostra todas as reservas da base de dados
        [AllowAnonymous] //permite a visualização das reservas da base de dados, sem a necessidade de autenticação
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservas.Include(r => r.Vendedor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas
                .Include(r => r.Vendedor)
                .FirstOrDefaultAsync(m => m.ReservaId == id);
            if (reservas == null)
            {
                return NotFound();
            }

            return View(reservas);
        }

        // GET: Reservas/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["Bancas"] = new MultiSelectList(_context.Bancas, "BancaId", "NomeIdentificadorBanca");

                var vendedores = await _context.Vendedores.ToListAsync();
                ViewBag.VendedoresList = new SelectList(vendedores, "UtilizadorId", "NISS");

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocorreu um erro ao gerar a página: " + ex.Message);
                throw;
            }
          
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("UtilizadorId,DataInicio,DataFim,EstadoActualReserva")] Reservas reserva, List<int> selectedBancasIds)
        {
            if (ModelState.IsValid)
            {
                reserva.ListaBancas = selectedBancasIds.Select(id => _context.Bancas.FirstOrDefault(b => b.BancaId == id)).ToList();

                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Bancas"] = new MultiSelectList(_context.Bancas, "BancaId", "NomeIdentificadorBanca", selectedBancasIds);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas.FindAsync(id);
            if (reservas == null)
            {
                return NotFound();
            }

            ViewData["UtilizadorId"] = new SelectList(_context.Set<Utilizadores>(), "UtilizadorId", "CC", reservas.Vendedor);

            return View(reservas);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservaId,UtilizadorId,DataInicio,DataFim,DataCriacao,EstadoActualReserva")] Reservas reservas)
        {
            if (id != reservas.ReservaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservasExists(reservas.ReservaId))
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

            ViewData["UtilizadorId"] = new SelectList(_context.Set<Utilizadores>(), "UtilizadorId", "CC", reservas.Vendedor);

            return View(reservas);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas
                .Include(r => r.Vendedor)
                .FirstOrDefaultAsync(m => m.ReservaId == id);
            if (reservas == null)
            {
                return NotFound();
            }

            return View(reservas);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservas = await _context.Reservas.FindAsync(id);
            if (reservas != null)
            {
                _context.Reservas.Remove(reservas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservasExists(int id)
        {
            return _context.Reservas.Any(e => e.ReservaId == id);
        }
    }
}
