using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class ResultadoSopaItem
    {
        
        [Required]
        public int IdItem { get; set; }

        [ForeignKey("IdItem")]
        public Item Item { get; set; }

        [Required]
        public int IdResultadoSopa { get; set; }

        [ForeignKey("IdResultadoSopa")]
        public ResultadoSopa ResultadoSopa { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue)]//não pode doar valor negativo
        public double Quantidade { get; set; }
    }
}
