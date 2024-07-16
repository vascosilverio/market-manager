using System.Collections.Generic;

namespace market_manager.Models
{
    // Classe que representa o modelo de uma matriz
    public class MatrixModel
    {
        // Propriedades da matriz
        public int X { get; set; }
        public int Y { get; set; }
        // Lista de bancas
        public List<Bancas> Bancas { get; set; }
    }
}
