using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static market_manager.Models.Bancas;

namespace market_manager.Models
{
    public class Notificacoes
    {

        [Key]
        public int NotificacaoId { get; set; }

        [ForeignKey(nameof(Vendedor))]
        public int VendedorId { get; set; }
        public Vendedores Vendedor { get; set; }


        [ForeignKey(nameof(Gestor))]
        public int GestorId { get; set; }
        public Gestores Gestor { get; set; }

        public EstadoNotificacao EstadoActualNotificacao { get; set; } = EstadoNotificacao.Enviada;
        
        [DataType(DataType.Date)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;


        public enum EstadoNotificacao
        {
            /// <summary>
            /// Enumerações permitidas para o estado de uma notificação
            /// </summary>
            Vista,
            Enviada
        }
    }
}
