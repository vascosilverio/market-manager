using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using market_manager.Data;
using market_manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using market_manager.Utils;

namespace market_manager.Controllers
{
    [Authorize]
    public class BancasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly int PageSize = 5;

        public BancasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Lista as bancas, com opções de pesquisa, filtros e paginação
        /// </summary>
        /// <param name="searchString">Termo de pesquisa</param>
        /// <param name="currentFilter">Filtro atual</param>
        /// <param name="sortOrder">Ordem de ordenação</param>
        /// <param name="pageNumber">Número da página</param>
        /// <param name="categoria">Filtro de categoria</param>
        /// <param name="estado">Filtro de estado</param>
        /// <returns>View com a lista de bancas</returns>
        public async Task<IActionResult> Index(string searchString = "", string currentFilter = "", string sortOrder = "", int? pageNumber = null, Bancas.CategoriaProdutos? categoria = null, Bancas.EstadoBanca? estado = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CategoriaSortParm"] = sortOrder == "Categoria" ? "categoria_desc" : "Categoria";
            ViewData["EstadoSortParm"] = sortOrder == "Estado" ? "estado_desc" : "Estado";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategoria"] = categoria;
            ViewData["CurrentEstado"] = estado;

            var bancas = _context.Bancas
                .Include(b => b.Reservas)
                    .ThenInclude(r => r.Utilizador)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                bancas = bancas.Where(s => s.NomeIdentificadorBanca.Contains(searchString));
            }

            if (categoria.HasValue)
            {
                bancas = bancas.Where(b => b.CategoriaBanca == categoria.Value);
            }

            if (estado.HasValue)
            {
                bancas = bancas.Where(b => b.EstadoAtualBanca == estado.Value);
            }

            bancas = sortOrder switch
            {
                "name_desc" => bancas.OrderByDescending(s => s.NomeIdentificadorBanca),
                "Categoria" => bancas.OrderBy(s => s.CategoriaBanca),
                "categoria_desc" => bancas.OrderByDescending(s => s.CategoriaBanca),
                "Estado" => bancas.OrderBy(s => s.EstadoAtualBanca),
                "estado_desc" => bancas.OrderByDescending(s => s.EstadoAtualBanca),
                _ => bancas.OrderBy(s => s.NomeIdentificadorBanca),
            };

            ViewData["Categorias"] = new SelectList(Enum.GetValues(typeof(Bancas.CategoriaProdutos)));
            ViewData["Estados"] = new SelectList(Enum.GetValues(typeof(Bancas.EstadoBanca)));

            int pageSize = PageSize;
            return View(await PaginatedList<Bancas>.CreateAsync(bancas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// Mostra os detalhes de uma banca
        /// </summary>
        /// <param name="id">ID da banca</param>
        /// <returns>View com os detalhes da banca</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banca = await _context.Bancas
                .Include(b => b.Reservas)
                    .ThenInclude(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.BancaId == id);

            if (banca == null)
            {
                return NotFound();
            }

            return View(banca);
        }

        /// <summary>
        /// Mostra o formulário para criar uma nova banca
        /// </summary>
        /// <returns>View com o formulário de criação de banca</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Cria uma nova banca
        /// </summary>
        /// <param name="banca">Dados da banca</param>
        /// <param name="FotografiaBanca">Arquivo de imagem da banca</param>
        /// <returns>Redirecionamento para a lista de bancas</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("NomeIdentificadorBanca,CategoriaBanca,LarguraAux,ComprimentoAux,LocalizacaoX,LocalizacaoY,EstadoAtualBanca")] Bancas banca, IFormFile FotografiaBanca)
        {
            if (await _context.Bancas.AnyAsync(b => b.NomeIdentificadorBanca == banca.NomeIdentificadorBanca))
            {
                ModelState.AddModelError("NomeIdentificadorBanca", "Já existe uma banca com este identificador.");
                return View(banca);
            }

            string nomeFotoBanca = "";
            bool haFotoBanca = false;

            if (FotografiaBanca == null)
            {
                ModelState.AddModelError("", "Fornecimento de uma fotografia da banca é obrigatório.");
                return View(banca);
            }
            else
            {
                if (!(FotografiaBanca.ContentType == "image/png" || FotografiaBanca.ContentType == "image/jpeg"))
                {
                    ModelState.AddModelError("", "Insira a fotografia no formato png ou jpeg.");
                    return View(banca);
                }
                else
                {
                    haFotoBanca = true;

                    Guid g = Guid.NewGuid();
                    nomeFotoBanca = g.ToString();
                    string extensao = Path.GetExtension(FotografiaBanca.FileName);

                    nomeFotoBanca += extensao;
                    banca.FotografiaBanca = nomeFotoBanca;
                    ModelState.Remove("FotografiaBanca");
                }
            }
            if (ModelState.IsValid)
            {
                banca.Largura = Convert.ToDecimal(banca.LarguraAux.Replace('.', ','));
                banca.Comprimento = Convert.ToDecimal(banca.ComprimentoAux.Replace('.', ','));

                _context.Add(banca);
                await _context.SaveChangesAsync();

                if (haFotoBanca)
                {
                    try
                    {
                        string nomePastaOndeGuardarImagem = _webHostEnvironment.WebRootPath;
                        nomePastaOndeGuardarImagem = Path.Combine(nomePastaOndeGuardarImagem, "FotosBancas");
                        if (!Directory.Exists(nomePastaOndeGuardarImagem))
                        {
                            Directory.CreateDirectory(nomePastaOndeGuardarImagem);
                        }
                        string nomeFinalFotografiaBanca = Path.Combine(nomePastaOndeGuardarImagem, nomeFotoBanca);
                        using var streamFotografiaBanca = new FileStream(nomeFinalFotografiaBanca, FileMode.CreateNew);
                        await FotografiaBanca.CopyToAsync(streamFotografiaBanca);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Ocorreu um erro ao salvar o arquivo: " + ex.Message);
                        return View(banca);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(banca);
        }

        /// <summary>
        /// Mostra o formulário para editar uma banca
        /// </summary>
        /// <param name="id">ID da banca</param>
        /// <returns>View com o formulário de edição de banca</returns>
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banca = await _context.Bancas.FindAsync(id);

            if (banca == null)
            {
                return NotFound();
            }

            banca.LarguraAux = banca.Largura.ToString("0.00").Replace(",", ".");
            banca.ComprimentoAux = banca.Comprimento.ToString("0.00").Replace(",", ".");
            ViewData["FotografiaBanca"] = banca.FotografiaBanca;

            return View(banca);
        }

        /// <summary>
        /// Edita uma banca
        /// </summary>
        /// <param name="id">ID da banca</param>
        /// <param name="banca">Dados atualizados da banca</param>
        /// <returns>Redirecionamento para a lista de bancas</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("BancaId,NomeIdentificadorBanca,CategoriaBanca,LarguraAux,ComprimentoAux,LocalizacaoX,LocalizacaoY,EstadoAtualBanca")] Bancas banca)
        {
            if (id != banca.BancaId)
            {
                return NotFound();
            }

            if (await _context.Bancas.AnyAsync(b => b.NomeIdentificadorBanca == banca.NomeIdentificadorBanca && b.BancaId != banca.BancaId))
            {
                ModelState.AddModelError("NomeIdentificadorBanca", "Já existe uma banca com este identificador.");
                return View(banca);
            }

            ModelState.Remove("FotografiaBanca");

            if (ModelState.IsValid)
            {
                try
                {
                    banca.Largura = Convert.ToDecimal(banca.LarguraAux.Replace('.', ','));
                    banca.Comprimento = Convert.ToDecimal(banca.ComprimentoAux.Replace('.', ','));

                    var existingBanca = await _context.Bancas.AsNoTracking().FirstOrDefaultAsync(b => b.BancaId == id);

                    var fotografiaBancaInput = Request.Form.Files["FotografiaBancaInput"];
                    if (fotografiaBancaInput != null && fotografiaBancaInput.Length > 0)
                    {
                        string nomePastaOndeGuardarImagem = _webHostEnvironment.WebRootPath;
                        nomePastaOndeGuardarImagem = Path.Combine(nomePastaOndeGuardarImagem, "FotosBancas");

                        if (!Directory.Exists(nomePastaOndeGuardarImagem))
                        {
                            Directory.CreateDirectory(nomePastaOndeGuardarImagem);
                        }

                        Guid g = Guid.NewGuid();
                        string extensao = Path.GetExtension(fotografiaBancaInput.FileName);
                        string nomeFotoBanca = $"{g}{extensao}";
                        string nomeFinalFotografiaBanca = Path.Combine(nomePastaOndeGuardarImagem, nomeFotoBanca);

                        if (existingBanca != null && !string.IsNullOrEmpty(existingBanca.FotografiaBanca))
                        {
                            var oldFilePath = Path.Combine(nomePastaOndeGuardarImagem, existingBanca.FotografiaBanca);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        using var streamFotografiaBanca = new FileStream(nomeFinalFotografiaBanca, FileMode.Create);
                        await fotografiaBancaInput.CopyToAsync(streamFotografiaBanca);

                        banca.FotografiaBanca = nomeFotoBanca;
                    }
                    else
                    {
                        if (existingBanca != null)
                        {
                            banca.FotografiaBanca = existingBanca.FotografiaBanca;
                        }
                    }

                    _context.Update(banca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BancasExists(banca.BancaId))
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
            return View(banca);
        }

        /// <summary>
        /// Mostra a página de confirmação de exclusão de uma banca
        /// </summary>
        /// <param name="id">ID da banca</param>
        /// <returns>View com a confirmação de exclusão de banca</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banca = await _context.Bancas
                .FirstOrDefaultAsync(m => m.BancaId == id);

            if (banca == null)
            {
                return NotFound();
            }

            return View(banca);
        }

        /// <summary>
        /// Exclui uma banca
        /// </summary>
        /// <param name="id">ID da banca</param>
        /// <returns>Redirecionamento para a lista de bancas</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banca = await _context.Bancas
                .Include(b => b.Reservas)
                .FirstOrDefaultAsync(b => b.BancaId == id);

            if (banca == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas
                .Where(r => r.ListaBancas.Any(b => b.BancaId == id))
                .ToListAsync();

            if (banca.Reservas != null && banca.Reservas.Any())
            {
                ModelState.AddModelError("", "Não é possível excluir a banca porque existem reservas associadas a ela.");
                return View(banca);
            }

            if (!string.IsNullOrEmpty(banca.FotografiaBanca))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "FotosBancas", banca.FotografiaBanca);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Bancas.Remove(banca);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica se uma banca existe
        /// </summary>
        /// <param name="id">ID da banca</param>
        /// <returns>True se a banca existe, false caso contrário</returns>
        private bool BancasExists(int id)
        {
            return _context.Bancas.Any(e => e.BancaId == id);
        }

        /// <summary>
        /// Obtém as localizações das bancas
        /// </summary>
        /// <returns>JSON com as localizações das bancas</returns>
        [HttpGet]
        public async Task<IActionResult> GetBancasLocalizacoes()
        {
            var bancas = await _context.Bancas
                .Select(b => new
                {
                    b.BancaId,
                    b.LocalizacaoX,
                    b.LocalizacaoY,
                    b.EstadoAtualBanca
                })
                .ToListAsync();

            return Json(bancas);
        }
    }
}