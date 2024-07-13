using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_manager.Models
{
    public class Bancas
    {

        [Key]
        public int BancaId { get; set; }

        [Required(ErrorMessage = "Introduza o nome da banca.")]
        [StringLength(5)]
        [Display(Name = "Identificador da Banca")]
        public string? NomeIdentificadorBanca { get; set; }

        [Required(ErrorMessage = "Introduza a categoria de produtos da banca.")]
        [Display(Name = "Categoria da Banca")]
        [EnumDataType(typeof(CategoriaProdutos))]
        public CategoriaProdutos CategoriaBanca { get; set; }

        [Required(ErrorMessage = "Introduza a coordenada X de localização da banca.")]
        [Display(Name = "Coordenada X da localização da banca no mercado.")]
        public int LocalizacaoX { get; set; }

        [Required(ErrorMessage = "Introduza a coordenada X de localização da banca.")]
        [Display(Name = "Coordenada Y da localização da banca no mercado.")]
        public int LocalizacaoY { get; set; }
      
        [Required(ErrorMessage = "Introduza o estado atual da banca.")]
        [EnumDataType(typeof(EstadoBanca))]
        [Display(Name = "Estado da Banca.")]
        public EstadoBanca EstadoAtualBanca { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Introduza a largura da banca com máximo de 3 dígitos de unidades e máximo de 2 dígitos de precisão")]
        [RegularExpression("^[0-9]{0,2}[.,]?[0-9]{0,2}$", ErrorMessage = "só aceita dígitos numéricos separados por um ponto, ou uma vírgula.")]
        [Display(Name = "Largura em metros.")]
        public string LarguraAux { get; set; }
        public decimal Largura { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Introduza o comprimento da banca com máximo de 3 dígitos de unidades e máximo de 2 dígitos de precisão")]
        [RegularExpression("^[0-9]{0,2}[.,]?[0-9]{0,2}$", ErrorMessage = "só aceita dígitos numéricos separados por um ponto, ou uma vírgula.")]
        [Display(Name = "Comprimento em metros.")]
        public string ComprimentoAux { get; set; }
        public decimal Comprimento { get; set; }

        [Required(ErrorMessage = "Deve inserir uma fotografia da banca.")]
        [Display(Name = "Fotografia da Banca.")]
        public string? FotografiaBanca { get; set; }

        public ICollection<Reservas>? Reservas { get; set; }

        
        public enum EstadoBanca
        {
            /// <summary>
            /// Enumerações permitidas para o estado de um registo de vendedor
            /// </summary>
            Ocupada,
            Livre,
            Manutenção
        }

        public enum CategoriaProdutos
        {
            /// <summary>
            /// Enumerações permitidas para as categorias de produtos de uma banca
            /// </summary>
            Congelados,
            Refrigerados,
            Frescos,
            Secos,
            Peixe,
            Carne 
        }

    }
}
