using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Utilizadores : IdentityUser
    {
        
        public Utilizadores() { 
            ListaReservas = new HashSet<Reservas>();
            ListaNotificacoes = new HashSet<Notificacoes>();
        }

        [Required(ErrorMessage = "Deve inserir a sua data de nascimento.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Nascimento")]
        public DateOnly DataNascimento { get; set; }

        [Required(ErrorMessage = "Deve inserir o seu primeiro nome.")]
        [StringLength(20, ErrorMessage = "Primeiro nome não deve ter mais do que 20 caracteres.")]
        [Display(Name = "Primeiro Nome")]
        public string PrimeiroNome { get; set; }

        [Required(ErrorMessage = "Deve inserir o seu último nome.")]
        [StringLength(20, ErrorMessage = "Último nome não deve ter mais do que 20 caracteres.")]
        [Display(Name = "Último Nome")]
        public string UltimoNome { get; set; }
       
        [Required(ErrorMessage = "Deve inserir o seu número de telemóvel.")]
        [Phone]
        [Display(Name = "Número de Telemóvel")]
        public string NumeroTelemovel { get; set; }

        [Required(ErrorMessage = "Deve inserir a sua morada.")]
        [StringLength(100, ErrorMessage = "A morada não deve ter mais do que 100 caracteres.")]
        public string Morada { get; set; }

        [Required(ErrorMessage = "Deve inserir o seu código postal.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O código postal não deve ter mais do que 8 caracteres.")]
        [Display(Name = "Código Postal")]
        [RegularExpression("[0-9]{4}-[0-9]{3}", ErrorMessage = "O {0} deve ser escrito no formato 0000-000")]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "Deve inserir a sua localidade.")]
        [StringLength(50, ErrorMessage = "A localidade não deve ter mais do que 50 caracteres.")]
        public string Localidade { get; set; }

        [Required(ErrorMessage = "Deve inserir o seu número de contribuinte.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O número de contribuinte deve ter 9 caracteres.")]
        public string NIF { get; set; }

        [Required(ErrorMessage = "Deve inserir o seu número de identificação civil.")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "O número de identificação civil deve ter pelo menos 8 caracteres..")]
        public string CC { get; set; }

        public ICollection<Reservas> ListaReservas { get; set; }
        public ICollection<Notificacoes> ListaNotificacoes { get; set; }

    }
}
