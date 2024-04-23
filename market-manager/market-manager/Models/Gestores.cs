using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Gestores : Utilizadores
    {

        [DataType(DataType.Date)]
        [Display(Name = "Data de Admissão")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateOnly DataAdmissao { get; set; }

        [Required]
        [StringLength(20)]
        public string NumIdFuncionario { get; set; }
    }
}
