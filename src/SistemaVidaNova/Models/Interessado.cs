using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class Interessado
    {
        [Key]
        public int CodInteressado { get; set; }
        [Required]
        public int Nome { get; set; }
        
        public int Celular { get; set; }
        
        public int Telefone { get; set; }
        
        public int Email { get; set; }

        public List<Evento> Eventos { get; set; }
    }
}
