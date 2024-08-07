﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace market_manager.Models
{
    // Classe que representa o modelo de uma reserva
    public class Reservas
    {
        // Propriedades da reserva com as anotações de validação

        [Key]
        public int ReservaId { get; set; }

        [Required(ErrorMessage = "Introduza um utilizador.")]
        [Display(Name = "Utilizador")]
        public string UtilizadorId { get; set; }

        public virtual Utilizadores Utilizador { get; set; }

        [Required(ErrorMessage ="Introduza a data de início da reserva.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de início da Reserva")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Introduza a data de fim da reserva.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de fim da Reserva")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DataFim { get; set; }

        [HiddenInput]
        [DataType(DataType.Date)]
        [Display(Name = "Data de criação da Reserva")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [HiddenInput]
        [EnumDataType(typeof(EstadoReserva))]
        [Display(Name = "Estado da Reserva")]
        public EstadoReserva EstadoActualReserva { get; set; } = EstadoReserva.Pendente;

        // Coleção de bancas associadas a uma reserva
        public virtual ICollection<Bancas> ListaBancas { get; set; }

        [NotMapped]
        // Lista de bancas selecionadas
        public List<int> SelectedBancaIds { get; set; }

        // Enumerações permitidas para o estado de uma reserva
        public enum EstadoReserva
        {
            /// <summary>
            /// Enumerações permitidas para o estado de uma reserva.
            /// </summary>
            Aprovada,
            Recusada,
            Pendente,
            Concluida
        }

    }
}
