using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class EstoqueDTO
    {
        public int Id { get; set; }        
        public string Nome { get; set; }
        public string UnidadeDeMedida { get; set; }
        
        [Required]
        public double Quantidade { get; set; }
    }
}
