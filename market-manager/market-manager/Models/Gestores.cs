using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_manager.Models
{
    [Table("Gestores")]
    public class Gestores : Utilizadores
    {
        public Gestores() {
            ListaNotificacoes = new HashSet<Notificacoes>();
        }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Admissão")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateOnly DataAdmissao { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Número de Funcionário")]
        public string NumIdFuncionario { get; set; }

        public ICollection<Notificacoes> ListaNotificacoes { get; set; }
    }
}
