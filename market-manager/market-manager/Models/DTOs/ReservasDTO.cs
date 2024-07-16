using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static market_manager.Models.Reservas;

namespace market_manager.Models.DTOs
{
	public class ReservasDTO
	{
		public DateTime DataInicio { get; set; }

		public DateTime DataFim { get; set; }

		[EnumDataType(typeof(EstadoReserva))]
		public EstadoReserva EstadoActualReserva { get; set; } = EstadoReserva.Pendente;

		public List<int> SelectedBancaIds { get; set; }

	}
}
