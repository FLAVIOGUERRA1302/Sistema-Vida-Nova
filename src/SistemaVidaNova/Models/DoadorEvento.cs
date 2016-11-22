using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class DoadorEvento
    {
        [Key]
        public int CodDoador { get; set; }
        [Key]
        public int CodEvento { get; set; }

        [ForeignKey("CodDoador")]
        public Doador Doador { get; set; }

        [ForeignKey("CodEvento")]
        public Evento Evento { get; set; }
    }
}
