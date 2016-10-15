using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class ResultadoSopaDTO
    {

        
        public int Id { get; set; }

        [Required]
        public ModeloDeReceitaDTOR ModeloDeReceita { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }
        public List<ResultadoSopaItemDTO> Itens { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue)]//não pode doar valor negativo
        public double LitrosProduzidos { get; set; }
    }
}
