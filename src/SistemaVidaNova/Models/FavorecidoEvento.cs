using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class FavorecidoEvento
    {
        [Key]
        public int CodFavorecido { get; set; }
        [Key]
        public int CodEvento { get; set; }

        [ForeignKey("CodFavorecido")]
        public Favorecido Favorecido { get; set; }

        [ForeignKey("CodEvento")]
        public Evento Evento { get; set; }
    }
}
