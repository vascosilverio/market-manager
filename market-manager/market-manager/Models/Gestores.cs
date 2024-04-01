using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Gestores : Utilizadores
    {

        [Required]
        [StringLength(100)]
        public string? Cargo { get; set; }

        [Required]
        [StringLength(100)]
        public string? Departamento { get; set; }

        public DateTime? DataAdmissao { get; set; }

        [Required]
        [StringLength(20)]
        public string? NumeroIdentificacaoFuncionario { get; set; }

    }
}
