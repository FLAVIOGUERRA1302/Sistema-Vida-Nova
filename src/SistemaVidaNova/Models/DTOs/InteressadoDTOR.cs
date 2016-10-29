using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class InteressadoDTOR
    {
        [Required]
        public int Id { get; set; }
        
        public string Nome { get; set; }
        
        
    }
}
