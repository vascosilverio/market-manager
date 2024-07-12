using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using market_manager.Data;
using market_manager.Models;
using market_manager.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace market_manager.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly int PageSize = 5;


        public ReservasController(ApplicationDbContext context, UserManager<Utilizadores> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservas
        public async Task<IActionResult> Index(string searchString, string currentFilter, string sortOrder, int? pageNumber, Reservas.EstadoReserva? estado)
        {


            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["StateSortParm"] = sortOrder == "State" ? "state_desc" : "State";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var user = await _userManager.GetUserAsync(User);
            var isGestor = await _userManager.IsInRoleAsync(user, "Gestor");

            IQueryable<Reservas> reservas = _context.Reservas
                .Include(r => r.Utilizador)
                .Include(r => r.ListaBancas);

            if (!isGestor)
            {
                reservas = reservas.Where(r => r.UtilizadorId == user.Id);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                reservas = reservas.Where(s => s.Utilizador.NomeCompleto.Contains(searchString));
            }

            reservas = sortOrder switch
            {
                "date_desc" => reservas.OrderByDescending(s => s.DataInicio),
                "State" => reservas.OrderBy(s => s.EstadoActualReserva),
                "state_desc" => reservas.OrderByDescending(s => s.EstadoActualReserva),
                _ => reservas.OrderBy(s => s.DataInicio),
            };

            var reservasList = await reservas.ToListAsync();

            foreach (var reserva in reservasList)
            {
                if (reserva.EstadoActualReserva == Reservas.EstadoReserva.Aprovada && reserva.DataFim < DateTime.Today)
                {
                    reserva.EstadoActualReserva = Reservas.EstadoReserva.Concluida;
                    _context.Update(reserva);
                }
            }

            await _context.SaveChangesAsync();

            return View(await PaginatedList<Reservas>.CreateAsync(reservas.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Utilizador)
                .Include(r => r.ListaBancas)
                .FirstOrDefaultAsync(m => m.ReservaId == id);

            if (reserva == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var isGestor = await _userManager.IsInRoleAsync(user, "Gestor");

            if (!isGestor && reserva.UtilizadorId != user.Id)
            {
                return Forbid();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            var bancas = _context.Bancas.Where(b => b.EstadoAtualBanca == Bancas.EstadoBanca.Livre).ToList();
            ViewData["Bancas"] = bancas;
            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Vendedor")]
        public async Task<IActionResult> Create([Bind("DataInicio,DataFim,SelectedBancaIds")] Reservas reserva)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var existingReservasCount = await _context.Reservas
                    .CountAsync(r => r.UtilizadorId == user.Id);

                if (existingReservasCount >= 2)
                {
                    ModelState.AddModelError("", "Já tem duas reservas feitas.");
                    ViewData["Bancas"] = _context.Bancas.Where(b => b.EstadoAtualBanca == Bancas.EstadoBanca.Livre).ToList();
                    return View(reserva);
                }

                // verifica se o utilizador ja tem uma reserva associada a alguma das bancas escolhidas
                var existingReservas = await _context.Reservas
                    .Where(r => r.UtilizadorId == user.Id && r.ListaBancas.Any(b => reserva.SelectedBancaIds.Contains(b.BancaId)))
                    .ToListAsync();

                if (existingReservas.Any())
                {
                    ModelState.AddModelError("", "Já tem uma reserva associada a esta banca.");
                    ViewData["Bancas"] = _context.Bancas.Where(b => b.EstadoAtualBanca == Bancas.EstadoBanca.Livre).ToList();
                    return View(reserva);
                }

                reserva.UtilizadorId = user.Id;
                reserva.EstadoActualReserva = Reservas.EstadoReserva.Pendente;
                reserva.DataCriacao = DateTime.Now;

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

            ViewData["Bancas"] = _context.Bancas.Where(b => b.EstadoAtualBanca == Bancas.EstadoBanca.Livre).ToList();
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.ListaBancas)
                .FirstOrDefaultAsync(r => r.ReservaId == id);

            if (reserva == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var isGestor = await _userManager.IsInRoleAsync(user, "Gestor");

            if (!isGestor)
            {
                ViewData["UtilizadorId"] = user.Id;
            }
            else
            {
                ViewData["UtilizadorId"] = new SelectList(_context.Utilizadores, "Id", "NomeCompleto", reserva.UtilizadorId);
            }

            ViewData["Bancas"] = _context.Bancas.Where(b => b.EstadoAtualBanca == Bancas.EstadoBanca.Livre).ToList();

            return View(reserva);
        }

            // POST: Reservas/Edit/5
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservaId,DataInicio,DataFim,SelectedBancaIds,EstadoActualReserva")] Reservas reserva)
        {
            if (id != reserva.ReservaId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var isGestor = await _userManager.IsInRoleAsync(user, "Gestor");


            var existingReserva = await _context.Reservas
                .Include(r => r.ListaBancas)
                .FirstOrDefaultAsync(r => r.ReservaId == id);

            if(!isGestor)
            {
                reserva.UtilizadorId = user.Id;
                reserva.EstadoActualReserva = existingReserva.EstadoActualReserva;
            }

            if (existingReserva == null)
            {
                return NotFound();
            }

            if (!isGestor && existingReserva.UtilizadorId != user.Id)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingReserva.DataInicio = reserva.DataInicio;
                    existingReserva.DataFim = reserva.DataFim;

                    if (isGestor)
                    {   
                        existingReserva.EstadoActualReserva = reserva.EstadoActualReserva;
                    }

                    if (reserva.SelectedBancaIds != null && reserva.SelectedBancaIds.Any())
                    {
                        existingReserva.ListaBancas = await _context.Bancas
                            .Where(b => reserva.SelectedBancaIds.Contains(b.BancaId))
                            .ToListAsync();
                    }

                    _context.Update(existingReserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservasExists(reserva.ReservaId))
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
            ViewData["ListaBancas"] = new MultiSelectList(_context.Bancas, "BancaId", "NomeIdentificadorBanca", reserva.SelectedBancaIds);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Utilizador)
                .Include(r => r.ListaBancas)
                .FirstOrDefaultAsync(m => m.ReservaId == id);

            if (reserva == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var isGestor = await _userManager.IsInRoleAsync(user, "Gestor");

            if (!isGestor && reserva.UtilizadorId != user.Id)
            {
                return Forbid();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var isGestor = await _userManager.IsInRoleAsync(user, "Gestor");

            if (!isGestor && reserva.UtilizadorId != user.Id)
            {
                return Forbid();
            }

            if (reserva.EstadoActualReserva == Reservas.EstadoReserva.Aprovada)
            {
                ModelState.AddModelError("", "Não pode apagar um reserva aprovada.");
                return View(reserva);
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> ChangeState(int id, Reservas.EstadoReserva newState)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            reserva.EstadoActualReserva = newState;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ReservasExists(int id)
        {
            return _context.Reservas.Any(e => e.ReservaId == id);
        }
    }
}