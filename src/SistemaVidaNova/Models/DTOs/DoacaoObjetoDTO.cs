using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class DoacaoObjetoDTO
    {
        
        public int Id { get; set; }

        [Required]
        
        public DoadorDTOR Doador { get; set; }
        [Required]
        public DateTime DataDaDoacao { get; set; }
        [Required]
        public DateTime DataDeRetirada { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }

        [Required]       
        public EnderecoDTO Endereco { get; set; }
    }
}
