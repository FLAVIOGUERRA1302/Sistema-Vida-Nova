using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class ModeloDeReceitaItemDTO
    {
        [Required]
        public ItemDTOR Item { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue)]//não pode doar valor negativo
        public double Quantidade { get; set; }

    }
}
