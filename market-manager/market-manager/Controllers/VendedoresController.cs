using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using market_manager.Data;
using market_manager.Models;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace market_manager.Controllers
{
    [Authorize] // Qualquer tarefa desta classe só pode ser efetuada por pessoas autorizadas (e autenticadas)
                //exceto se se criar uma exceção
    public class VendedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public VendedoresController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Vendedores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vendedores.ToListAsync());
        }

        // GET: Vendedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendedores = await _context.Vendedores
                .FirstOrDefaultAsync(m => m.UtilizadorId == id);
            if (vendedores == null)
            {
                return NotFound();
            }

            return View(vendedores);
        }

        // GET: Vendedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vendedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NISS,DataNascimento,PrimeiroNome,UltimoNome,Telemovel,Morada,CodigoPostal,Localidade,NIF,CC")] Vendedores vendedor, IFormFile DocumentoCartaoComerciante)
        {

            string nomeDocumentoCartaoComerciante = "";
            bool haDocumentoCartaoComerciante = false;

            if (DocumentoCartaoComerciante == null)
            {
                ModelState.AddModelError("", "Fornecimento da cópia do documento de Cartão Comerciante é obrigatória.");
                return View(vendedor);
            }
            else
            {
                if (!(DocumentoCartaoComerciante.ContentType == "image/png"
                ||
                    DocumentoCartaoComerciante.ContentType == "image/jpeg"
                ||
                    DocumentoCartaoComerciante.ContentType == "application/pdf"))
                {
                    ModelState.AddModelError("", "Insira a documentação no formato png, jpeg ou pdf.");
                    return View(vendedor);
                }
                else
                {
                    haDocumentoCartaoComerciante = true;

                    Guid g = Guid.NewGuid();
                    nomeDocumentoCartaoComerciante = g.ToString();
                    //obter a extensão
                    string extensao = Path.GetExtension(DocumentoCartaoComerciante.FileName);

                    nomeDocumentoCartaoComerciante += extensao;
                    //adicionar o nome do ficheiro ao objeto que vem do browser
                    vendedor.DocumentoCartaoComerciante = nomeDocumentoCartaoComerciante;
                    ModelState.Remove("DocumentoCartaoComerciante");
                }
            }

            // avalia se os dados que chegam da view estão de acordo com o model
            if (ModelState.IsValid)
            {
                // adiciona os dados vindos da View à BD
                vendedor.EstadoActualRegisto = Vendedores.EstadoRegisto.Pendente;
                _context.Add(vendedor); ;
                //faz o commit na BD
                await _context.SaveChangesAsync();

                // se há ficheiro de imagem, vamos guardar no disco rígido do servidor.
                if (haDocumentoCartaoComerciante)
                {
                    try
                    {
                        string nomePastaOndeGuardarImagem = _webHostEnvironment.WebRootPath;
                        //adicionar pasta imagens
                        nomePastaOndeGuardarImagem = Path.Combine(nomePastaOndeGuardarImagem, "DocumentosVendedor");
                        // e, existe a pasta imagens?
                        if (!Directory.Exists(nomePastaOndeGuardarImagem))
                        {
                            // se nao existe, cria-se
                            Directory.CreateDirectory(nomePastaOndeGuardarImagem);
                        }
                        // juntar o nome do ficheiro à sua localização
                        string nomeFinalDocumentoCartaoComerciante = Path.Combine(nomePastaOndeGuardarImagem, nomeDocumentoCartaoComerciante);
                        // guardar a imagem no disco rigido
                        using var streamCartaoComerciante = new FileStream(nomeFinalDocumentoCartaoComerciante, FileMode.CreateNew);
                        await DocumentoCartaoComerciante.CopyToAsync(streamCartaoComerciante);
                    }
                    catch (Exception ex)
                    {
                        // Tratar a exceção de forma adequada
                        // Por exemplo, registrar o erro em um log e exibir uma mensagem de erro para o usuário
                        ModelState.AddModelError("", "Ocorreu um erro ao salvar o arquivo: " + ex.Message);
                        return View(vendedor);
                    }
                }
                // redireciona o user para a página index
                return RedirectToAction(nameof(Index));
            }
            //se cheguei aqui é porque alguma coisa correu mal :'(
            //volta à View com os dados fornecidos pela View
            return View(vendedor);
        }


        // GET: Vendedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendedores = await _context.Vendedores.FindAsync(id);
            if (vendedores == null)
            {
                return NotFound();
            }
            return View(vendedores);
        }

        // POST: Vendedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NISS,EstadoActualRegisto,UtilizadorId,DataNascimento,PrimeiroNome,UltimoNome,Telemovel,Morada,CodigoPostal,Localidade,NIF,CC")] Vendedores vendedor, IFormFile DocumentoCartaoComerciante)
        {

            if (id != vendedor.UtilizadorId)
            {
                return NotFound();
            }

            string nomeDocumentoCartaoComerciante = "";
            bool haDocumentoCartaoComerciante = false;

            if (DocumentoCartaoComerciante == null)
            {
                ModelState.AddModelError("", "Fornecimento da cópia do documento de Cartão Comerciante é obrigatória.");
                return View(vendedor);
            }
            else
            {
                if (!(DocumentoCartaoComerciante.ContentType == "image/png"
                ||
                    DocumentoCartaoComerciante.ContentType == "image/jpeg"
                ||
                    DocumentoCartaoComerciante.ContentType == "application/pdf"))
                {
                    ModelState.AddModelError("", "Insira a documentação no formato png, jpeg ou pdf.");
                    return View(vendedor);
                }
                else
                {
                    haDocumentoCartaoComerciante = true;

                    Guid g = Guid.NewGuid();
                    nomeDocumentoCartaoComerciante = g.ToString();
                    //obter a extensão
                    string extensao = Path.GetExtension(DocumentoCartaoComerciante.FileName);

                    nomeDocumentoCartaoComerciante += extensao;
                    //adicionar o nome do ficheiro ao objeto que vem do browser
                    vendedor.DocumentoCartaoComerciante = nomeDocumentoCartaoComerciante;
                    ModelState.Remove("DocumentoCartaoComerciante");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ce)
                {
                    if (!VendedoresExists(vendedor.UtilizadorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // se há ficheiro de imagem, vamos guardar no disco rígido do servidor.
                if (haDocumentoCartaoComerciante)
                {
                    try
                    {
                        string nomePastaOndeGuardarImagem = _webHostEnvironment.WebRootPath;
                        //adicionar pasta imagens
                        nomePastaOndeGuardarImagem = Path.Combine(nomePastaOndeGuardarImagem, "DocumentosVendedor");
                        // e, existe a pasta imagens?
                        if (!Directory.Exists(nomePastaOndeGuardarImagem))
                        {
                            // se nao existe, cria-se
                            Directory.CreateDirectory(nomePastaOndeGuardarImagem);
                        }
                        // juntar o nome do ficheiro à sua localização
                        string nomeFinalDocumentoCartaoComerciante = Path.Combine(nomePastaOndeGuardarImagem, nomeDocumentoCartaoComerciante);
                        // guardar a imagem no disco rigido
                        using var streamCartaoComerciante = new FileStream(nomeFinalDocumentoCartaoComerciante, FileMode.CreateNew);
                        await DocumentoCartaoComerciante.CopyToAsync(streamCartaoComerciante);
                    }
                    catch (Exception ex)
                    {
                        // Tratar a exceção de forma adequada
                        // Por exemplo, registrar o erro em um log e exibir uma mensagem de erro para o usuário
                        ModelState.AddModelError("", "Ocorreu um erro ao salvar o arquivo: " + ex.Message);
                        return View(vendedor);
                    }
                }
                // redireciona o user para a página index
                return RedirectToAction(nameof(Index));

            }
            //se cheguei aqui é porque alguma coisa correu mal :'(
            //volta à View com os dados fornecidos pela View
            return View(vendedor);
        }

        // GET: Vendedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendedores = await _context.Vendedores
                .FirstOrDefaultAsync(m => m.UtilizadorId == id);
            if (vendedores == null)
            {
                return NotFound();
            }

            return View(vendedores);
        }

        // POST: Vendedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendedores = await _context.Vendedores.FindAsync(id);
            if (vendedores != null)
            {
                _context.Vendedores.Remove(vendedores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendedoresExists(int id)
        {
            return _context.Vendedores.Any(e => e.UtilizadorId == id);
        }
    }
}
