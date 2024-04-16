using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Gestores : Utilizadores
    {
        public DateTime? DataAdmissao { get; set; }

        [Required]
        [StringLength(20)]
        public string? NumeroIdentificacaoFuncionario { get; set; }

    }
}
