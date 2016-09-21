using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTO
{
    public class InteressadoDTO
    {
        
        public int Id { get; set; }
        [Required]
        public int Nome { get; set; }
        
        public int Celular { get; set; }
        
        public int Telefone { get; set; }

        [EmailAddress]
        public int Email { get; set; }

        public List<EventoDTO> Eventos { get; set; }
    }
}
