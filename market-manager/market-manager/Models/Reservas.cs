using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Reservas
    {

        [Key]
        public int ReservaId { get; set; }

        [ForeignKey("Utilizador")]
        public int UtilizadorId { get; set; }
        public Utilizadores Utilizador { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Estado { get; set; }

        public ICollection<Bancas> Bancas { get; set; }
        public ICollection<Notificacoes> Notificacoes { get; set; }

    }
}
