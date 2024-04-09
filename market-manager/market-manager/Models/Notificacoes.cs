using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static market_manager.Models.Bancas;

namespace market_manager.Models
{
    public class Notificacoes
    {

        [Key]
        public int NotificacaoId { get; set; }

        [ForeignKey(nameof(Utilizador))]
        public string? DestinatarioId { get; set; }
        public Utilizadores? Utilizador { get; set; }

        [EnumDataType(typeof(EstadoNotificacao))]
        public EstadoNotificacao? EstadoActualNotificacao { get; set; } = EstadoNotificacao.Enviada;
        public DateTime? DataCriacao { get; set; } = DateTime.Now;

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
