using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class ModeloDeReceitaItem
    {
        
        [Required]
        public int IdItem { get; set; }

        [ForeignKey("IdItem")]
        public Item Item { get; set; }

        [Required]
        public int IdModeloDeReceita { get; set; }

        [ForeignKey("IdModeloDeReceita")]
        public ModeloDeReceita ModeloDeReceita  { get; set; }

        [Required]
        public double Quantidade { get; set; }
    }
}
