using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTO
{
   
    public class FamiliaDTO
    {

        
        public int id { get; set; }
                
        [MaxLength(200)]
        public string Nome { get; set; }        
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }

        public EnderecoDTO Endereco { get; set; }
    }
    
}
