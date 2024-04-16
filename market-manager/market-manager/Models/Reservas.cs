using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Reservas
    {
        public Reservas() { 
            ListaBancas = new HashSet<Bancas>();
        }

        [Key]
        public int ReservaId { get; set; }

        [ForeignKey(nameof(Utilizador))]
        public string UtilizadorId { get; set; }
        public Utilizadores? Utilizador { get; set; }

        [Required(ErrorMessage ="Introduza a data de início da reserva.")]
        [Display(Name = "Data de início da Reserva.")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Introduza a data de fim da reserva.")]
        [Display(Name = "Data de fim da Reserva.")]
        public DateTime DataFim { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public EstadoReserva? EstadoActualReserva { get; set; } = EstadoReserva.Pendente;

        public ICollection<Bancas> ListaBancas { get; set; }

        public enum EstadoReserva
        {
            /// <summary>
            /// Enumerações permitidas para o estado de uma reserva.
            /// </summary>
            Aprovada,
            Recusada,
            Pendente
        }

    }
}
