using static market_manager.Models.Bancas;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace market_manager.Models.DTOs
{
	public class BancasDTO
	{
		[Required(ErrorMessage = "Introduza o nome de localização da banca.")]
		[StringLength(5)]
		[Display(Name = "Identificador da Banca")]
		public string? NomeIdentificadorBanca { get; set; }

		[Required(ErrorMessage = "Introduza a categoria de produtos da banca.")]
		[Display(Name = "Categoria da Banca")]
		[EnumDataType(typeof(CategoriaProdutos))]
		public CategoriaProdutos CategoriaBanca { get; set; }

		[NotMapped]
		[Required(ErrorMessage = "Introduza a largura da banca com máximo de 3 dígitos de unidades e máximo de 2 dígitos de precisão")]
		[RegularExpression("^[0-9]{0,2}[.,]?[0-9]{0,2}$", ErrorMessage = "só aceita dígitos numéricos separados por um ponto, ou uma vírgula.")]
		public decimal Largura { get; set; }

		[NotMapped]
		[Required(ErrorMessage = "Introduza o comprimento da banca com máximo de 3 dígitos de unidades e máximo de 2 dígitos de precisão")]
		[RegularExpression("^[0-9]{0,2}[.,]?[0-9]{0,2}$", ErrorMessage = "só aceita dígitos numéricos separados por um ponto, ou uma vírgula.")]
		public decimal Comprimento { get; set; }

		[Required(ErrorMessage = "Introduza a coordenada X de localização da banca.")]
		public int LocalizacaoX { get; set; }

		[Required(ErrorMessage = "Introduza a coordenada Y de localização da banca.")]
		public int LocalizacaoY { get; set; }

		[Required(ErrorMessage = "Introduza o estado atual da banca.")]
		[EnumDataType(typeof(EstadoBanca))]
		public EstadoBanca EstadoAtualBanca { get; set; }

		[Required(ErrorMessage = "Deve inserir uma fotografia da banca.")]
		[Display(Name = "Fotografia da Banca.")]
		public string? FotografiaBanca { get; set; }
	}
}
