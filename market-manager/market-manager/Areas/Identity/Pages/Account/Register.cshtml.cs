// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using market_manager.Data;
using market_manager.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace market_manager.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///  esta variável irá conter o "destino" a ser aplicado pela aplicação, quando 
        ///  após o "registo" a aplicação pretender ser repocisionada
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// se se adicionar as chaves de autenticação por "providers" externos, aqui
        /// serão listados por esta variável (por exemplo login com conta google, twitter, etc)
        /// ver: https://go.microsoft.com/fwlink/?LinkID=532715
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// "inner class"
        /// define os atributos a serem enviados/recebidos para/da interface
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Email do utilizador
            /// </summary>
            [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
            [EmailAddress(ErrorMessage = "Escreva um {0} válido, por favor")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            /// Password de acesso ao sistemam, pelo utilizador
            /// </summary>
            [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
            [StringLength(20, ErrorMessage = "A {0} tem de ter no mínimo {2}, e no máximo {1} como número de caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "A password e a sua confirmação não coincidem.")]
            public string ConfirmPassword { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Vendedores Vendedor { get; set; }
        }

 

        /// <summary>
        /// este método reage ao verbo HTTP GET
        /// </summary>
        /// <param name="returnUrl">o endereço onde "estávamos" quando foi feito o pedido para
        /// nos registarmos
        /// </param>
        /// <returns></returns>
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }





        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
          //  ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                //estamos aqui a verdadeiramente guardar os dados da autenticação na base de dados
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // **********************************************************
                    // vamos escrever na BD os dados do Gestor
                    // na prática, quero aguarda na BD os dados
                    // do atributo 'input.Gestor'
                    // **********************************************************

                    // vamos guardar o valor do atributo
                    // que fará a 'ponte' entre a BD
                    // de autenticação e a BD do 'negócio'
                    Input.Vendedor.UserId = user.Id;

                    try
                    {
                        // guardar os dados na BD
                        _context.Add(Input.Vendedor);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        // há que registar os dados do que aconteceu mal, para se reparar o problema

                        // se cheguei aqui é pq não se conseguiu escrever os dados do Professor na BD
                        // há que tomar uma decisão sobre o que fazer...
                        
                        // Sugestão:
                        // - guardar os dados da exceção num ficheiro de 'log' no disco rígido do servidor
                        // - guardar os dados da exceção numa tabela da BD
                        // - apagar o 'utilizador' criado dentro do try
                        //
                        //- notificar a pessoa que está a interagir com a aplicação do sucedido
                        // - redirecionar a pessoa para uma página de erro

                        _logger.LogInformation(ex.ToString()); // na consola do servidor não do computador
                        
                        throw;
                    }
                    

                    // **********************************************************

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    // envia email para o utilizador com o código
                    // de validação do email inserido
                    // SÓ APÓS a aceitação desta tarefa o utilizador pode entrar na app
                    // isto é apenas a interface, tem de ser configurado primeiro
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
