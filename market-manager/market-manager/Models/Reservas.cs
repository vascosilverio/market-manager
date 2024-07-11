using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace market_manager.Models
{
    public class Reservas
    {

        [Key]
        public int ReservaId { get; set; }

        public string UtilizadorId { get; set; }

        public Utilizadores Utilizador { get; set; }

        [Required(ErrorMessage ="Introduza a data de início da reserva.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de início da Reserva.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateOnly DataInicio { get; set; }

        [Required(ErrorMessage = "Introduza a data de fim da reserva.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de fim da Reserva.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateOnly DataFim { get; set; }

        [HiddenInput]
        [DataType(DataType.Date)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [HiddenInput]
        [EnumDataType(typeof(EstadoReserva))]
        public EstadoReserva EstadoActualReserva { get; set; } = EstadoReserva.Pendente;

        public ICollection<Bancas> ListaBancas { get; set; }

        [NotMapped]
        public List<int> SelectedBancaIds { get; set; }

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
