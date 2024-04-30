using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static market_manager.Models.Bancas;

namespace market_manager.Models
{
    public class Notificacoes
    {

        [Key]
        public int NotificacaoId { get; set; }

        public virtual Vendedores? Vendedor { get; set; }

        public virtual Gestores? Gestor { get; set; }

        public EstadoNotificacao EstadoActualNotificacao { get; set; } = EstadoNotificacao.Enviada;
        
        [DataType(DataType.Date)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public string Conteudo { get; set; }

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
