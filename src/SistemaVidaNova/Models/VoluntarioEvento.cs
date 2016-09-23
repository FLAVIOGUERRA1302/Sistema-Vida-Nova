using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class VoluntarioEvento
    {
        [Key]
        public int IdVoluntario { get; set; }
        [Key]
        public int CodEvento { get; set; }

        [ForeignKey("IdVoluntario")]
        public Voluntario Voluntario { get; set; }

        [ForeignKey("CodEvento")]
        public Evento Evento { get; set; }
    }
}
