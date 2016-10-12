using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class DoacaoDinheiro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CodDoador { get; set; }

        [ForeignKey("CodDoador")]
        public Doador Doador { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue)]//não pode doar valor negativo
        public Double Valor { get; set; }
    }
}
