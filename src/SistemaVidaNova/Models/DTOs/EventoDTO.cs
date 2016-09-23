using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTO
{
    public class EventoDTO
    {
        
        public int id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "O {0} tem que ter no mínimo {2} e no máximo {1} caracteres", MinimumLength = 6)]
        public string title { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "O {0} tem que ter no máximo {1} caracteres")]
        public string descricao { get; set; }
        [Required]
        public string color { get; set; }
        [Required]
        public string textColor { get; set; }
        [Required]
        public DateTime start { get; set; }
        [Required]
        public DateTime end { get; set; }
        [Required]
        public double valorDeEntrada { get; set; }
        
        public double valorArrecadado { get; set; }

        [StringLength(5000, ErrorMessage = "O {0} tem que ter no máximo {1} caracteres")]
        public string relato { get; set; }
        public string url { get; set; }
        
        
    }
}
