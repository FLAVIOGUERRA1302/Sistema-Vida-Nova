using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaVidaNova.Models
{
    public class Favorecido 
    {
        [Key]
        public int CodFavorecido { get; set; }
                
        [Required]
        public string Nome { get; set; }
        public string Apelido { get; set; }
        
        public string Cpf { get; set; }
        
        public string Rg { get; set; }
        
        [Required]
        public String Sexo { get; set; }
        
        public DateTime? DataNascimento { get; set; }
        

        [Required]
        public DateTime DataDeCadastro { get; set; }

        public ICollection<ConhecimentoProficional> ConhecimentosProfissionais { get; set; }
        [Required]
        public string IdUsuario { get; set; }
        
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
                
        public Familia Familia { get; set; }


    }
}
