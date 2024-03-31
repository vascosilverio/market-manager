using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class Utilizadores
    {

        [Key]
        public int UtilizadorId { get; set; }
        public string NomeUtilizador { get; set; }
        public string PalavraPasse { get; set; }
        public string TipoUtilizador { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Morada { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidade { get; set; }
        public string Estado { get; set; }
        public string NIF { get; set; }

        public ICollection<Reservas> Reservas { get; set; }
        public ICollection<Notificacoes> NotificacoesRecebidas { get; set; }
        public ICollection<Notificacoes> NotificacaoesEnviadas { get; set; }

    }
}
