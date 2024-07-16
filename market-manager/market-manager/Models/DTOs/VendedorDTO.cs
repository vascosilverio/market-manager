namespace market_manager.Models.DTOmodels
{
    // Classe que representa o DTO de Vendedor com os atributos necessários para a criação de um vendedor
    // Contem também as anotações de validação dos atributos
    public class VendedorDTO
    {
        public string NISS { get; set; }

        public string DocumentoCartaoComerciante { get; set; }

        public string DocumentoCC { get; set; }
    }

}
