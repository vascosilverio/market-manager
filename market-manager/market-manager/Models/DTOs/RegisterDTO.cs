using System.ComponentModel.DataAnnotations;

namespace market_manager.Models.DTOs
{
    public class RegisterDTO
    {

        public string Email { get; set; }

        public string Password { get; set; }

        public string NomeCompleto { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Telemovel { get; set; }

        public string Morada { get; set; }

        public string CodigoPostal { get; set; }

        public string Localidade { get; set; }

        public string CC { get; set; }

        public string NIF { get; set; }
    }
}