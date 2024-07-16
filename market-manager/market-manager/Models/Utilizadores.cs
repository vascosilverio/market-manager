using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    // Classe que representa o modelo de utilizadores
    public class Utilizadores : IdentityUser
    {
    
        // Propriedades da classe
        public string Role { get; set; }

        [Required(ErrorMessage = "Deve inserir a sua data de nascimento.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Deve inserir o seu nome completo.")]
        [Display(Name = "Nome completo")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "Deve inserir o seu número de telemóvel.")]
        [Phone]
        [StringLength(9)]
        [RegularExpression("^9[1236][0-9]{7}$", ErrorMessage = "o {0} só aceita 9 dígitos.")]
        [Display(Name = "Número de Telemóvel")]
        public string Telemovel { get; set; }

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

        // Enumerações permitidas para o estado de um registo de vendedor
        public enum EstadoRegisto
        {
            /// <summary>
            /// Enumerações permitidas para o estado de um registo de vendedor
            /// </summary>
            Aprovado,
            Recusado,
            Pendente
        }

        // Lista de reservas associadas a um utilizador
        public ICollection<Reservas> ListaReservas { get; set; }

    }
}
