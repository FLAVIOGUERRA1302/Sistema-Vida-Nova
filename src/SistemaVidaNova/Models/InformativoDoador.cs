using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class InformativoDoador
    {
        [Key]
        public int CodDoador { get; set; }
        [Key]
        public int IdInformativo { get; set; }

        [ForeignKey("CodDoador")]
        public Doador Doador { get; set; }

        [ForeignKey("IdInformativo")]
        public Informativo Informativo { get; set; }
    }
}
