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
        public string Nome { get; set; }
        
        public string Celular { get; set; }
        
        public string Telefone { get; set; }
        [Required]
        public string Email { get; set; }

        public List<Evento> Eventos { get; set; }
    }
}
