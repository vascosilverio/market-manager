using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using market_manager.Data;
using market_manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace market_manager.Controllers
{
    [Authorize]
    public class BancasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BancasController(ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Bancas
        public async Task<IActionResult> Index()
        {
            ViewBag.MatrixSize = 5;
            return View(await _context.Bancas.ToListAsync());
        }

        [HttpPost]
        public IActionResult UpdateCoordinates([FromBody] MatrixModel model)
        {
            // Process the coordinates here if needed
            return Json(new { success = true });
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
        public async Task<IActionResult> Create([Bind("NomeIdentificadorBanca,CategoriaBanca,LarguraAux,ComprimentoAux,LocalizacaoX,LocalizacaoY,EstadoActualBanca")] Bancas banca, IFormFile FotografiaBanca)
        {

            string nomeFotoBanca = "";
            bool haFotoBanca = false;

            if (FotografiaBanca == null)
            {
                ModelState.AddModelError("", "Fornecimento de uma fotografia da banca é obrigatório.");
                return View(banca);
            }
            else
            {
                if (!(FotografiaBanca.ContentType == "image/png"
                ||
                    FotografiaBanca.ContentType == "image/jpeg"))
                {
                    ModelState.AddModelError("", "Insira a fotogradia no formato png ou jpeg.");
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
                _context.Add(banca); ;
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
