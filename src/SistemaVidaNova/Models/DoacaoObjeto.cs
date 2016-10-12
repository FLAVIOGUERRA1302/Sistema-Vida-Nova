using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class DoacaoObjeto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CodDoador { get; set; }

        [ForeignKey("CodDoador")]
        public Doador Doador { get; set; }
        [Required]
        public DateTime DataDaDoacao { get; set; }
        [Required]
        public DateTime DataDeRetirada { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }

        [Required]
        public int IdEndereco { get; set; }

        [Required]
        [ForeignKey("IdEndereco")]
        public Endereco Endereco { get; set; }
    }
}
