using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static market_manager.Models.Reservas;

namespace market_manager.Models.DTOs
{
	// Classe que representa o DTO de Reservas com os atributos necessários para a criação de uma reserva
	// Contem também as anotações de validação dos atributos
	public class ReservasDTO
	{
		[Required(ErrorMessage = "Introduza a data de início da reserva.")]
		[DataType(DataType.Date)]
		[Display(Name = "Data de início da Reserva.")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
		public DateTime DataInicio { get; set; }

		[Required(ErrorMessage = "Introduza a data de fim da reserva.")]
		[DataType(DataType.Date)]
		[Display(Name = "Data de fim da Reserva.")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
		public DateTime DataFim { get; set; }

		[Required]
		public string UtilizadorId { get; set; }

		[EnumDataType(typeof(EstadoReserva))]
		public EstadoReserva EstadoActualReserva { get; set; } = EstadoReserva.Pendente;

		public List<int> SelectedBancaIds { get; set; }

	}
}
