using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace market_manager.Models
{
    [Table("Vendedores")]
    public class Vendedores : Utilizadores
    {
        public Vendedores() {
            ListaReservas = new HashSet<Reservas>();
            ListaNotificacoes = new HashSet<Notificacoes>();
        }

        [Required(ErrorMessage = "Deve inserir o seu número de identificação da segurança social.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O seu número de identificação da segurança social deve ter 11 caracteres.")]
        [Display(Name = "Data de Identificação da Segurança Social")]
        public string NISS { get; set; }

        [Display(Name = "Estado de Registo")]
        [HiddenInput]
        public EstadoRegisto EstadoActualRegisto{ get; set; }

        [Required(ErrorMessage = "Deve inserir uma cópia do seu cartão de comerciante.")]
        [Display(Name = "Fotocópia do Cartão de Comerciante")]
        public string? DocumentoCartaoComerciante { get; set; }

        public enum EstadoRegisto
        {
            /// <summary>
            /// Enumerações permitidas para o estado de um registo de vendedor
            /// </summary>
            Aprovado,
            Recusado,
            Pendente
        }
        public ICollection<Notificacoes> ListaNotificacoes { get; set; }
        public ICollection<Reservas> ListaReservas { get; set; }
    }
}
