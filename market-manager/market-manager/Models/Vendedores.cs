using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace market_manager.Models
{
    public class Vendedores : Utilizadores
    {
        public Vendedores() {

        }

        [Required(ErrorMessage = "Deve inserir o seu número de identificação da segurança social.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O seu número de identificação da segurança social deve ter 11 caracteres.")]
        [Display(Name = "Data de Identificação da Segurança Social")]
        public string NISS { get; set; }

        [Display(Name = "Estado de Registo")]
        
        [HiddenInput]
        public EstadoRegisto EstadoActualRegisto{ get; set; } = EstadoRegisto.Pendente;

        [Required(ErrorMessage = "Deve inserir uma cópia do seu cartão de comerciante.")]
        [Display(Name = "Fotocópia do Cartão de Comerciante")]
        public string DocumentoCartaoComerciante { get; set; }

        [Required(ErrorMessage = "Deve inserir uma cópia do seu cartão de cidadão.")]
        [Display(Name = "Fotocópia do Cartão de Cidadão")]
        public string DocumentoCC { get; set; }

        public enum EstadoRegisto
        {
            /// <summary>
            /// Enumerações permitidas para o estado de um registo de vendedor
            /// </summary>
            Aprovado,
            Recusado,
            Pendente
        }

    }
}
