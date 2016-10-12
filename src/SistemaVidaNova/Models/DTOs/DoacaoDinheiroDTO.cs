using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class DoacaoDinheiroDTO
    {
        
        public int Id { get; set; }

        [Required]
        
        public DoadorDTOR Doador { get; set; }

        [Required]
        public DateTime DataDaDoacao { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue)]//não pode doar valor negativo
        public Double Valor { get; set; }
    }
}
