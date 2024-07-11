
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using market_manager.Data;
using market_manager.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace market_manager.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
               .Include(r => r.Utilizador)  
               .Include(r => r.ListaBancas)
               .ToListAsync();

            var applicationDbContext = _context.Reservas.Include(r => r.Utilizador);

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
                .Include(r => r.Utilizador)
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

                var utilizadores = _context.Users.ToList();
                ViewBag.Utilizadores = new SelectList(utilizadores, "Id", "UserName");

                var bancas = _context.Bancas.ToList();
                ViewBag.Bancas = new MultiSelectList(bancas, "BancaId", "NomeIdentificadorBanca");


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

        public async Task<IActionResult> Create([Bind("ReservaId,UtilizadorId,DataInicio,DataFim,SelectedBancaIds")] Reservas reserva)
        {
            if (ModelState.IsValid)
            {
                if (reserva.SelectedBancaIds != null && reserva.SelectedBancaIds.Any())
                {
                    reserva.ListaBancas = await _context.Bancas
                        .Where(b => reserva.SelectedBancaIds.Contains(b.BancaId))
                        .ToListAsync();
                }

                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var utilizadores = _context.Users.ToList();
            ViewBag.Utilizadores = new SelectList(utilizadores, "Id", "UserName", reserva.UtilizadorId);

            var bancas = _context.Bancas.ToList();
            ViewBag.Bancas = new MultiSelectList(bancas, "BancaId", "NomeIdentificadorBanca");

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

            ViewData["UtilizadorId"] = new SelectList(_context.Set<Utilizadores>(), "UtilizadorId", "CC", reservas.Utilizador);

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

            ViewData["UtilizadorId"] = new SelectList(_context.Set<Utilizadores>(), "UtilizadorId", "CC", reservas.Utilizador);

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
                .Include(r => r.Utilizador)
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
