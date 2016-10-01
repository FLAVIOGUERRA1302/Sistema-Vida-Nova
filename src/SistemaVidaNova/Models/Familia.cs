using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
   
    public class Familia
    {

        [Key]
        public int CodFamilia { get; set; }
                
        [MaxLength(200)]
        public string Nome { get; set; }        
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }

        [Required]
        public int IdEndereco { get; set; }

        [ForeignKey("IdEndereco")]
        public Endereco Endereco { get; set; }

        [Required]
        public int CodFavorecido { get; set; }

        [ForeignKey("CodFavorecido")]
        public Favorecido Favorecido { get; set; }
    }
    
}
