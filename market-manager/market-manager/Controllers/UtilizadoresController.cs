using market_manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using market_manager.Data;
using market_manager.Models;


namespace market_manager.Controllers
{
    /// <summary>
    /// Controler dos utilizador
    /// </summary>
    [Authorize]
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Utilizadores> _signInManager;

        public UtilizadoresController(UserManager<Utilizadores> usermanager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = usermanager;
            _roleManager = roleManager;
            _context = context;
        }

        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilizadores.ToListAsync());
        }

        /// <summary>
        /// Detalhes de um utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Details(string? id)
        {
            // se o id estiver a null, retornar not found
            if (id == null)
            {
                return NotFound();
            }

            // ir bsucar à base de dados esse utilizador
            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);

            // retornar para a view o utilizador
            return View(utilizadores);
        }


        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,PrimeiroNome,UltimoNome,DataNascimento,Telemovel,Morada,CodigoPostal,Localidade,NIF,CC,Email,UserName")] Utilizadores utilizador)
        {
            if (ModelState.IsValid)
            {
                // verificações
                if (await _context.Utilizadores.AnyAsync(u => u.Email == utilizador.Email))
                {
                    ModelState.AddModelError("NomeCompleto", "Um utilizador com o mesmo e-mail já existe.");
                    return View(utilizador);
                }

                _context.Add(utilizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador);
        }


        /// <summary>
        /// Editar um utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(string? id)
        {
            // se o id estiver a null, retornar not found
            if (id == null)
            {
                return NotFound();
            }

            // ir buscar à base de dados esse utilizador
            var utilizador = await _context.Utilizadores.FindAsync(id);

            // retornar para a view o utilizador
            return View(utilizador);
        }

        /// <summary>
        /// Editar um utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <param name="utilizador"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,NomeCompleto,DataNascimento,Telemovel,Morada,CodigoPostal,Localidade,NIF,CC,Email,UserName")] Utilizadores utilizador, String role)
        {
            // verificar se o ID vem a null
            if (utilizador is null)
            {
                return NotFound();
            }

            // verificar se o ModelState é válido
            if (ModelState.IsValid)
            {
                // se for, tentar editar o utilizador
                try
                {
                    // Ober o utilizador com o _userManager
                    var utilizadorAtualizado = await _userManager.FindByIdAsync(id);
                    if (utilizadorAtualizado == null)
                    {
                        return NotFound();
                    }

                    // Atribuir ao utilizador os novos dados que vieram da View
                    utilizadorAtualizado.NomeCompleto = utilizador.NomeCompleto;
                    utilizadorAtualizado.UserName = utilizador.UserName;
                    utilizadorAtualizado.Email = utilizador.Email;
                    utilizadorAtualizado.Morada = utilizador.Morada;
                    utilizadorAtualizado.CodigoPostal = utilizador.CodigoPostal;

                    // Atualizar o utilizador com os novos dados e verificar se teve sucesso
                    var resultado = await _userManager.UpdateAsync(utilizadorAtualizado);

                    // Se a atualização dos dados do utilizador tiveram sucesso, alterar a Role
                    if (resultado.Succeeded)
                    {
                        if (role != null)
                        {
                            var roleAntiga = await _userManager.GetRolesAsync(utilizadorAtualizado);
                            await _userManager.RemoveFromRolesAsync(utilizadorAtualizado, roleAntiga);
                            await _userManager.AddToRoleAsync(utilizadorAtualizado, role);
                        }

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // se não, devolver erro para o ModelState
                        ModelState.AddModelError("", "Erro ao tentar atualizar o utilizador.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            // retornar para a view o utilizador
            return View(utilizador);
        }

        /// <summary>
        /// Remover um utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(string? id)
        {
            // se o ID view a null, devolver not found
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores.FindAsync(id);

            if (utilizador == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas.Where(r => r.UtilizadorId == id).ToListAsync();

            if (reservas.Any())
            {
                ModelState.AddModelError("", "Não é possível apagar o utilizador porque existem reservas associadas a ele.");
                return View(utilizador);
            }

            return View(utilizador);
        }

        /// <summary>
        /// Remoção de um utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            // verificar se existem utilizadores que podem ser removidos
            if (_context.Utilizadores == null)
            {
                return Problem("Entity set 'market_manager.Utilizadores'  is null.");
            }

            // ir buscar à base de dados o utilizador
            var utilizadores = await _context.Utilizadores.FindAsync(id);

            if (utilizadores != null)
            {
                // remover o utilizador da base de dados
                _context.Utilizadores.Remove(utilizadores);
            }

            // guardar as alterações da base de dados
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadoresExists(string id)
        {
            return (_context.Utilizadores?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Utilizadores model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                user.NomeCompleto = model.NomeCompleto;
                // Update other user profile fields

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao atualizar o perfil do usuário.");
                }
            }

            return View("Profile", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }
            else
            {
                ModelState.AddModelError("", "Erro ao alterar a senha.");
            }

            return View("Profile", user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Delete user reservations
            var reservas = await _context.Reservas.Where(r => r.UtilizadorId == user.Id).ToListAsync();
            _context.Reservas.RemoveRange(reservas);

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Erro ao excluir a conta do usuário.");
            }

            return View("Profile", user);
        }


    }
}