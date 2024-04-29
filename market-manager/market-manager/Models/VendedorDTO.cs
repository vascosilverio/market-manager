using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace market_manager.Models
{
    public class VendedorDTO
    {
        public string NISS { get; set; }

        public string DocumentoCartaoComerciante { get; set; }

        public string DocumentoCC { get; set; }
    }
}
