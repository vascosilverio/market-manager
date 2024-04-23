using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Notificacoes
    {

        [Key]
        public int NotificacaoId { get; set; }

        [ForeignKey(nameof(Utilizador))]
        public int DestinatarioId { get; set; }
        public Utilizadores Utilizador { get; set; }

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
