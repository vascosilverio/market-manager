using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using market_manager.Data;
using market_manager.Models;
using market_manager.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace market_manager.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;

        public ReservasController(ApplicationDbContext context, UserManager<Utilizadores> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Lista as reservas, com opções de pesquisa e paginação
        /// </summary>
        /// <param name="sortOrder">Ordem de ordenação</param>
        /// <param name="currentFilter">Filtro atual</param>
        /// <param name="searchString">Termo de pesquisa</param>
        /// <param name="pageNumber">Número da página</param>
        /// <returns>View com a lista de reservas</returns>
        public async Task<IActionResult> Index(string searchString = "", string currentFilter = "", string sortOrder = "", int? pageNumber = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var reservas = from r in _context.Reservas
                           .Include(r => r.Utilizador)
                           .Include(r => r.ListaBancas)
                           select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                reservas = reservas.Where(s => s.Utilizador.NomeCompleto.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    reservas = reservas.OrderByDescending(s => s.DataInicio);
                    break;
                default:
                    reservas = reservas.OrderBy(s => s.DataInicio);
                    break;
            }

            int pageSize = 3;

            return View(await PaginatedList<Reservas>.CreateAsync(reservas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// Mostra os detalhes de uma reserva
        /// </summary>
        /// <param name="id">ID da reserva</param>
        /// <returns>View com os detalhes da reserva</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservas == null)
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

            return View(reserva);
        }

        /// <summary>
        /// Mostra o formulário para criar uma nova reserva
        /// </summary>
        /// <returns>View com o formulário de criação de reserva</returns>
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o user com ID '{_userManager.GetUserId(User)}'.");
            }

            ViewData["Bancas"] = new SelectList(_context.Bancas.Where(b => b.EstadoAtualBanca == Bancas.EstadoBanca.Livre), "BancaId", "NomeIdentificadorBanca");
            ViewData["Utilizadores"] = new SelectList(_context.Users, "Id", "NomeCompleto");
          
            return View();
        }

        /// <summary>
        /// Cria uma nova reserva
        /// </summary>
        /// <param name="reserva">Dados da reserva</param>
        /// <returns>Redirecionamento para a lista de reservas</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservaId,DataInicio,DataFim,SelectedBancaIds")] Reservas reserva)
        {
            var user = await _userManager.GetUserAsync(User);
            reserva.UtilizadorId = user.Id;
            reserva.EstadoActualReserva = Reservas.EstadoReserva.Pendente;
            reserva.Utilizador = user;
            reserva.ListaBancas = await _context.Bancas.Where(b => reserva.SelectedBancaIds.Contains(b.BancaId)).ToListAsync();

            if (ModelState.IsValid)
            {
                

                foreach (var banca in reserva.ListaBancas)
                {
                    banca.EstadoAtualBanca = Bancas.EstadoBanca.Ocupada;
                }

                _context.Add(reserva);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reserva criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
             ViewData["Bancas"] = new SelectList(_context.Bancas.Where(b => b.EstadoAtualBanca == Bancas.EstadoBanca.Livre), "BancaId", "NomeIdentificadorBanca", reserva.SelectedBancaIds);
            ViewData["Utilizadores"] = new SelectList(_context.Users, "Id", "NomeCompleto", reserva.UtilizadorId);
            return View(reserva);
        }

        /// <summary>
        /// Mostra o formulário para editar uma reserva
        /// </summary>
        /// <param name="id">ID da reserva</param>
        /// <returns>View com o formulário de edição de reserva</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservas == null)
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
            var todasBancas = await _context.Bancas.ToListAsync();
            ViewData["Bancas"] = todasBancas;
            ViewData["BancasDisponiveis"] = new MultiSelectList(_context.Bancas.Where(b => b.EstadoAtualBanca == Bancas.EstadoBanca.Livre || reserva.ListaBancas.Contains(b)), "BancaId", "NomeIdentificadorBanca", reserva.ListaBancas.Select(b => b.BancaId));
            return View(reserva);
        }

        /// <summary>
        /// Edita uma reserva
        /// </summary>
        /// <param name="id">ID da reserva</param>
        /// <param name="reservaAtualizada">Dados atualizados da reserva</param>
        /// <returns>Redirecionamento para a lista de reservas</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservaId,DataInicio,DataFim,SelectedBancaIds,EstadoActualReserva")] Reservas reservaAtualizada)
        {
            if (id != reservaAtualizada.ReservaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var reserva = await _context.Reservas
                        .Include(r => r.ListaBancas)
                        .FirstOrDefaultAsync(m => m.ReservaId == id);

                    if (reserva == null)
                    {
                        return NotFound();
                    }

                    reserva.DataInicio = reservaAtualizada.DataInicio;
                    reserva.DataFim = reservaAtualizada.DataFim;
                    reserva.EstadoActualReserva = reservaAtualizada.EstadoActualReserva;

                    var bancasAtuais = reserva.ListaBancas.ToList();
                    var bancasSelecionadas = await _context.Bancas.Where(b => reservaAtualizada.SelectedBancaIds.Contains(b.BancaId)).ToListAsync();

                    foreach (var banca in bancasAtuais.Except(bancasSelecionadas))
                    {
                        reserva.ListaBancas.Remove(banca);
                        banca.EstadoAtualBanca = Bancas.EstadoBanca.Livre;
                    }

                    foreach (var banca in bancasSelecionadas.Except(bancasAtuais))
                    {
                        reserva.ListaBancas.Add(banca);
                        banca.EstadoAtualBanca = Bancas.EstadoBanca.Ocupada;
                    }

                    if (reserva.EstadoActualReserva == Reservas.EstadoReserva.Aprovada)
                    {
                        foreach (var banca in reserva.ListaBancas)
                        {
                            banca.EstadoAtualBanca = Bancas.EstadoBanca.Ocupada;
                        }
                    }
                    else if (reserva.EstadoActualReserva == Reservas.EstadoReserva.Concluida)
                    {
                        foreach (var banca in reserva.ListaBancas)
                        {
                            banca.EstadoAtualBanca = Bancas.EstadoBanca.Livre;
                        }
                    }

                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Reserva atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservasExists(reservaAtualizada.ReservaId))
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
           
            ViewData["BancasDisponiveis"] = new MultiSelectList(_context.Bancas.Where(b => b.EstadoAtualBanca == Bancas.EstadoBanca.Livre || reservaAtualizada.ListaBancas.Contains(b)), "BancaId", "NomeIdentificadorBanca", reservaAtualizada.ListaBancas.Select(b => b.BancaId));
            return View(reservaAtualizada);
        }

        /// <summary>
        /// Mostra a página de confirmação de exclusão de uma reserva
        /// </summary>
        /// <param name="id">ID da reserva</param>
        /// <returns>View com a confirmação de exclusão de reserva</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservas == null)
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

            return View(reserva);
        }

        /// <summary>
        /// Exclui uma reserva
        /// </summary>
        /// <param name="id">ID da reserva</param>
        /// <returns>Redirecionamento para a lista de reservas</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservas'  is null.");
            }
            var reserva = await _context.Reservas
                .Include(r => r.ListaBancas)
                .FirstOrDefaultAsync(m => m.ReservaId == id);
            if (reserva != null)
            {
                foreach (var banca in reserva.ListaBancas)
                {
                    banca.EstadoAtualBanca = Bancas.EstadoBanca.Livre;
                }

                _context.Reservas.Remove(reserva);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Reserva apagada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica se uma reserva existe
        /// </summary>
        /// <param name="id">ID da reserva</param>
        /// <returns>True se a reserva existe, false caso contrário</returns>
        private bool ReservasExists(int id)
        {
            return (_context.Reservas?.Any(e => e.ReservaId == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeState(int id, Reservas.EstadoReserva newState)
        {
            var reserva = await _context.Reservas
                .Include(r => r.ListaBancas)
                .FirstOrDefaultAsync(r => r.ReservaId == id);

            if (reserva == null)
            {
                return NotFound();
            }

            reserva.EstadoActualReserva = newState;

            // Atualizar o estado das bancas
            foreach (var banca in reserva.ListaBancas)
            {
                if (newState == Reservas.EstadoReserva.Aprovada)
                {
                    banca.EstadoAtualBanca = Bancas.EstadoBanca.Ocupada;
                }
                else if (newState == Reservas.EstadoReserva.Concluida || newState == Reservas.EstadoReserva.Recusada )
                {
                    banca.EstadoAtualBanca = Bancas.EstadoBanca.Livre;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}