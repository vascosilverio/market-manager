using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Bancas
    {

        [Key]
        public int BancaId { get; set; }
        public string NomeBanca { get; set; }
        public string CategoriaProdutos { get; set; }
        public decimal Largura { get; set; }
        public decimal Comprimento { get; set; }
        public int LocalizacaoX { get; set; }
        public int LocalizacaoY { get; set; }
        public string Estado { get; set; }

        public ICollection<Reservas> Reservas { get; set; }

    }
}
