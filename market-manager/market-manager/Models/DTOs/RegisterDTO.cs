using System.ComponentModel.DataAnnotations;

namespace market_manager.Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string NomeCompleto { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string Telemovel { get; set; }

        [Required]
        public string Morada { get; set; }

        [Required]
        public string CodigoPostal { get; set; }

        [Required]
        public string Localidade { get; set; }

        [Required]
        public string CC { get; set; }

        [Required]
        public string NIF { get; set; }
    }
}