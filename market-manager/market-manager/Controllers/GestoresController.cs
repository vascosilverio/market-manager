using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using market_manager.Data;
using market_manager.Models;

namespace market_manager.Controllers
{
    public class GestoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GestoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gestores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gestores.ToListAsync());
        }

        // GET: Gestores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gestores = await _context.Gestores
                .FirstOrDefaultAsync(m => m.UtilizadorId == id);
            if (gestores == null)
            {
                return NotFound();
            }

            return View(gestores);
        }

        // GET: Gestores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gestores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataAdmissao,NumeroIdentificacaoFuncionario,UtilizadorId,DataNascimento,PrimeiroNome,UltimoNome,Telemovel,Morada,CodigoPostal,Localidade,NIF,CC")] Gestores gestor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gestor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gestor);
        }

        // GET: Gestores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gestores = await _context.Gestores.FindAsync(id);
            if (gestores == null)
            {
                return NotFound();
            }
            return View(gestores);
        }

        // POST: Gestores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DataAdmissao,NumeroIdentificacaoFuncionario,UtilizadorId,DataNascimento,PrimeiroNome,UltimoNome,Telemovel,Morada,CodigoPostal,Localidade,NIF,CC")] Gestores gestores)
        {
            if (id != gestores.UtilizadorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gestores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GestoresExists(gestores.UtilizadorId))
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
            return View(gestores);
        }

        // GET: Gestores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gestores = await _context.Gestores
                .FirstOrDefaultAsync(m => m.UtilizadorId == id);
            if (gestores == null)
            {
                return NotFound();
            }

            return View(gestores);
        }

        // POST: Gestores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gestores = await _context.Gestores.FindAsync(id);
            if (gestores != null)
            {
                _context.Gestores.Remove(gestores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GestoresExists(int id)
        {
            return _context.Gestores.Any(e => e.UtilizadorId == id);
        }
    }
}
