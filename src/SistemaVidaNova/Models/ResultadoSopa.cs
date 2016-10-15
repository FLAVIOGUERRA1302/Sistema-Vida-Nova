using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class ResultadoSopa
    {

        [Key]
        public int Id { get; set; }
        
        [Required]
        public int IdModeloDeReceita { get; set; }

        [ForeignKey("IdModeloDeReceita")]
        public ModeloDeReceita ModeloDeReceita { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }
        public List<ResultadoSopaItem> Itens { get; set; }

        [Required]
        public DateTime Data { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue)]//não pode doar valor negativo
        public double LitrosProduzidos { get; set; }
    }
}
