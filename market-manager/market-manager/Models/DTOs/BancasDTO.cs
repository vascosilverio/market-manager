using static market_manager.Models.Bancas;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace market_manager.Models.DTOs
{
	public class BancasDTO
	{
		public string? NomeIdentificadorBanca { get; set; }

		public CategoriaProdutos CategoriaBanca { get; set; }

		public decimal Largura { get; set; }

		public decimal Comprimento { get; set; }

		public int LocalizacaoX { get; set; }

		public int LocalizacaoY { get; set; }

		public EstadoBanca EstadoAtualBanca { get; set; }

		public string? FotografiaBanca { get; set; }
	}
}
