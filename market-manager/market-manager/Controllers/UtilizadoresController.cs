using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using market_manager.Data;
using market_manager.Models;
using Microsoft.EntityFrameworkCore;

namespace market_manager.Controllers
{
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;

        // Construtor que recebe o contexto da aplicação e o gestor de utilizadores como parâmetros
        public UtilizadoresController(ApplicationDbContext context, UserManager<Utilizadores> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Lista os utilizadores
        /// </summary>
        /// <returns>View com a lista de utilizadores</returns>
        public async Task<IActionResult> Index()
        {
            return _context.Utilizadores != null ?
                        View(await _context.Utilizadores.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Utilizadores'  is null.");
        }

        /// <summary>
        /// Mostra os detalhes de um utilizador
        /// </summary>
        /// <param name="id">ID do utilizador</param>
        /// <returns>View com os detalhes do utilizador</returns>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Utilizadores == null)
            {
                return NotFound();
            }

            // Utiliza o método FirstOrDefaultAsync para obter o primeiro utilizador cujo ID corresponde ao ID fornecido
            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            return View(utilizadores);
        }

        /// <summary>
        /// Mostra o formulário para criar um novo utilizador
        /// </summary>
        /// <returns>View com o formulário de criação de utilizador</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Cria um novo utilizador
        /// </summary>
        /// <param name="utilizadores">Dados do utilizador</param>
        /// <returns>Redirecionamento para a lista de utilizadores</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeCompleto,DataNascimento,Telemovel,Morada,CodigoPostal,Localidade,NIF,Email,UserName")] Utilizadores utilizadores, string password, string role)
        {
            if (ModelState.IsValid)
            {
                // Atribui um ID único ao utilizador
                utilizadores.Id = Guid.NewGuid().ToString();
                utilizadores.EmailConfirmed = true;
                utilizadores.Role = role;
                // Cria o utilizador na base de dados e atribui-lhe a função especificada
                await _userManager.CreateAsync(utilizadores, password);
                await _userManager.AddToRoleAsync(utilizadores, role);

                _context.Add(utilizadores);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Utilizador criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(utilizadores);
        }

        /// <summary>
        /// Mostra o formulário para editar um utilizador
        /// </summary>
        /// <param name="id">ID do utilizador</param>
        /// <returns>View com o formulário de edição de utilizador</returns>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Utilizadores == null)
            {
                return NotFound();
            }

            // Utiliza o método FindAsync para obter o utilizador cujo ID corresponde ao ID fornecido
            var utilizadores = await _context.Utilizadores.FindAsync(id);
            if (utilizadores == null)
            {
                return NotFound();
            }
            return View(utilizadores);
        }

        /// <summary>
        /// Edita um utilizador
        /// </summary>
        /// <param name="id">ID do utilizador</param>
        /// <param name="utilizadores">Dados atualizados do utilizador</param>
        /// <returns>Redirecionamento para a lista de utilizadores</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,NomeCompleto,DataNascimento,Telemovel,Morada,CodigoPostal,Localidade,NIF,Email,UserName")] Utilizadores utilizadores)
        {
            // Verifica se o ID fornecido corresponde ao ID do utilizador
            if (id != utilizadores.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizadores);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Utilizador atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Verifica se o utilizador existe
                    if (!UtilizadoresExists(utilizadores.Id))
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
            return View(utilizadores);
        }

        /// <summary>
        /// Mostra a página de confirmação de exclusão de um utilizador
        /// </summary>
        /// <param name="id">ID do utilizador</param>
        /// <returns>View com a confirmação de exclusão de utilizador</returns>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Utilizadores == null)
            {
                return NotFound();
            }

            // Utiliza o método FirstOrDefaultAsync para obter o primeiro utilizador cujo ID corresponde ao ID fornecido
            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            return View(utilizadores);
        }

        /// <summary>
        /// Exclui um utilizador
        /// </summary>
        /// <param name="id">ID do utilizador</param>
        /// <returns>Redirecionamento para a lista de utilizadores</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Utilizadores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Utilizadores'  is null.");
            }

            // Utiliza o método FindAsync para obter o utilizador cujo ID corresponde ao ID fornecido
            var utilizadores = await _context.Utilizadores.FindAsync(id);
            if (utilizadores != null)
            {
                try
                {
                    // Remove todas as reservas associadas ao utilizador
                    var reservas = _context.Reservas.Where(r => r.UtilizadorId == id);
                    _context.Reservas.RemoveRange(reservas);
                    _context.Utilizadores.Remove(utilizadores);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Utilizador apagado com sucesso!";
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Não é possível apagar o utilizador porque existem reservas associadas a ele.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica se um utilizador existe
        /// </summary>
        /// <param name="id">ID do utilizador</param>
        /// <returns>True se o utilizador existe, false caso contrário</returns>
        private bool UtilizadoresExists(string id)
        {
            return (_context.Utilizadores?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /// <summary>
        /// Mostra a página de perfil do utilizador autenticado
        /// </summary>
        /// <returns>View com os detalhes do perfil do utilizador</returns>
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Atualiza o perfil do utilizador autenticado
        /// </summary>
        /// <param name="utilizadores">Dados atualizados do utilizador</param>
        /// <returns>Redirecionamento para a página de perfil</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        // utiliza o objeto Utilizadores para receber os dados do utilizador
        public async Task<IActionResult> UpdateProfile([Bind("Id,NomeCompleto,DataNascimento,Telemovel,Morada,CodigoPostal,Localidade,NIF,Email")] Utilizadores utilizadores)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.NomeCompleto = utilizadores.NomeCompleto;
                    user.DataNascimento = utilizadores.DataNascimento;
                    user.Telemovel = utilizadores.Telemovel;
                    user.Morada = utilizadores.Morada;
                    user.CodigoPostal = utilizadores.CodigoPostal;
                    user.Localidade = utilizadores.Localidade;
                    user.NIF = utilizadores.NIF;
                    user.Email = utilizadores.Email;

                    await _userManager.UpdateAsync(user);
                    TempData["SuccessMessage"] = "Perfil atualizado com sucesso!";
                    return RedirectToAction(nameof(Profile));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadoresExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(utilizadores);
        }

        /// <summary>
        /// Exclui a conta do utilizador autenticado
        /// </summary>
        /// <returns>Redirecionamento para a página inicial</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                // Remove todas as reservas associadas ao utilizador
                var reservas = _context.Reservas.Where(r => r.UtilizadorId == user.Id);
                _context.Reservas.RemoveRange(reservas);
                await _userManager.DeleteAsync(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Conta apagada com sucesso!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Não é possível apagar a conta porque existem reservas associadas a ela.";
                return RedirectToAction(nameof(Profile));
            }
        }
    }
}