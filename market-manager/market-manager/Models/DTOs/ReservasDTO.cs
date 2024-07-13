using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static market_manager.Models.Reservas;

namespace market_manager.Models.DTOs
{
	public class ReservasDTO
	{
		[Required(ErrorMessage = "Introduza a data de início da reserva.")]
		[DataType(DataType.Date)]
		[Display(Name = "Data de início da Reserva.")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
		public DateOnly DataInicio { get; set; }

		[Required(ErrorMessage = "Introduza a data de fim da reserva.")]
		[DataType(DataType.Date)]
		[Display(Name = "Data de fim da Reserva.")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
		public DateOnly DataFim { get; set; }

		[Required]
		public string UtilizadorId { get; set; }

		[EnumDataType(typeof(EstadoReserva))]
		public EstadoReserva EstadoActualReserva { get; set; } = EstadoReserva.Pendente;

		public List<int> SelectedBancaIds { get; set; }

	}
}
