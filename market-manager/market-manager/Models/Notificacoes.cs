using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Notificacoes
    {

        [Key]
        public int NotificacaoId { get; set; }
        public string Tipo { get; set; }

        [ForeignKey("Destinatario")]
        public int DestinatarioId { get; set; }
        public Utilizadores Destinatario { get; set; }

        [ForeignKey("Registo")]
        public int RegistoId { get; set; }
        public Utilizadores Registo { get; set; }

        public string DestinatarioTipo { get; set; }

        [ForeignKey("Reserva")]
        public int? ReservaId { get; set; }
        public Reservas Reserva { get; set; }

        public string Estado { get; set; }
        public DateTime DataCriacao { get; set; }

    }
}
