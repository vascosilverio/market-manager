using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Bancas
    {

        [Key]
        public int BancaId { get; set; }

        [Required(ErrorMessage = "Introduza o nome de localização da banca.")]
        [StringLength(5)]
        [Display(Name = "Identificador da Banca")]
        public string? NomeIdentificadorBanca { get; set; }

        [Required(ErrorMessage = "Introduza a categoria de produtos da banca.")]
        [Display(Name = "Categoria da Banca")]
        public CategoriaProdutos CategoriaBanca { get; set; }

        [Required(ErrorMessage = "Introduza a largura da banca.")]
        [Range(0.1, double.MaxValue)]
        public decimal Largura { get; set; }

        [Required(ErrorMessage = "Introduza o comprimento da banca.")]
        [Range(0.1, double.MaxValue)]
        public decimal Comprimento { get; set; }

        [Required(ErrorMessage = "Introduza a coordenada X de localização da banca.")]
        public int LocalizacaoX { get; set; }

        [Required(ErrorMessage = "Introduza a coordenada X de localização da banca.")]
        public int LocalizacaoY { get; set; }

        [Required(ErrorMessage = "Introduza o estado atual da banca.")]
        public EstadoBanca EstadoActualBanca { get; set; } 

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
