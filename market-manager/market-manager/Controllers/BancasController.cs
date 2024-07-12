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

        public async Task<IActionResult> Index(string searchString, string currentFilter, string sortOrder, int? pageNumber, Bancas.CategoriaProdutos? categoria, Bancas.EstadoBanca? estado)
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
            return View(bancas); ;
        }

        

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banca = await _context.Bancas
                .Include(b => b.Reservas)
                .FirstOrDefaultAsync(m => m.BancaId == id);

            if (banca == null)
            {
                return NotFound();
            }

            return View(banca);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("NomeIdentificadorBanca,CategoriaBanca,LarguraAux,ComprimentoAux,LocalizacaoX,LocalizacaoY,EstadoActualBanca")] Bancas banca, IFormFile FotografiaBanca)
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
                    //obter a extensão
                    string extensao = Path.GetExtension(FotografiaBanca.FileName);

                    nomeFotoBanca += extensao;
                    //adicionar o nome do ficheiro ao objeto que vem do browser
                    banca.FotografiaBanca = nomeFotoBanca;
                    ModelState.Remove("FotografiaBanca");
                }
            }

            // avalia se os dados que chegam da view estão de acordo com o model
            if (ModelState.IsValid)
            {
                //transferir o valor de ComprimentoAux e LarguraAux para Comprimento e Largura
                banca.Largura = Convert.ToDecimal(banca.LarguraAux.Replace('.', ','));
                banca.Comprimento = Convert.ToDecimal(banca.ComprimentoAux.Replace('.', ','));

                // adiciona os dados vindos da View à BD
                _context.Add(banca);
                //faz o commit na BD
                await _context.SaveChangesAsync();

                // se há ficheiro de imagem, vamos guardar no disco rígido do servidor.
                if (haFotoBanca)
                {
                    try
                    {
                        string nomePastaOndeGuardarImagem = _webHostEnvironment.WebRootPath;
                        //adicionar pasta imagens
                        nomePastaOndeGuardarImagem = Path.Combine(nomePastaOndeGuardarImagem, "FotosBancas");
                        // e, existe a pasta imagens?
                        if (!Directory.Exists(nomePastaOndeGuardarImagem))
                        {
                            // se nao existe, cria-se
                            Directory.CreateDirectory(nomePastaOndeGuardarImagem);
                        }
                        // juntar o nome do ficheiro à sua localização
                        string nomeFinalFotografiaBanca = Path.Combine(nomePastaOndeGuardarImagem, nomeFotoBanca);
                        // guardar a imagem no disco rigido
                        using var streamFotografiaBanca = new FileStream(nomeFinalFotografiaBanca, FileMode.CreateNew);
                        await FotografiaBanca.CopyToAsync(streamFotografiaBanca);
                    }
                    catch (Exception ex)
                    {
                        // Tratar a exceção de forma adequada
                        // Por exemplo, registrar o erro em um log e exibir uma mensagem de erro para o usuário
                        ModelState.AddModelError("", "Ocorreu um erro ao salvar o arquivo: " + ex.Message);
                        return View(banca);
                    }
                }
                // redireciona o user para a página index
                return RedirectToAction(nameof(Index));
            }
            //se cheguei aqui é porque alguma coisa correu mal :'(
            //volta à View com os dados fornecidos pela View
            return View(banca);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("BancaId,NomeIdentificadorBanca,CategoriaBanca,LarguraAux,ComprimentoAux,LocalizacaoX,LocalizacaoY,EstadoActualBanca")] Bancas banca)
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
                    //transferir o valor de ComprimentoAux e LarguraAux para Comprimento e Largura
                    banca.Largura = Convert.ToDecimal(banca.LarguraAux.Replace('.', ','));
                    banca.Comprimento = Convert.ToDecimal(banca.ComprimentoAux.Replace('.', ','));

                    // Retrieve the existing banca entity from the database
                    var existingBanca = await _context.Bancas.AsNoTracking().FirstOrDefaultAsync(b => b.BancaId == id);

                    // Handle image file upload
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

                        // apagar a imagem antiga
                        if (existingBanca != null && !string.IsNullOrEmpty(existingBanca.FotografiaBanca))
                        {
                            var oldFilePath = Path.Combine(nomePastaOndeGuardarImagem, existingBanca.FotografiaBanca);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // guardar a imagem no disco rigido
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

            // Delete the image file
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

        private bool BancasExists(int id)
        {
            return _context.Bancas.Any(e => e.BancaId == id);
        }

    }
}