using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTO
{
    public class FavorecidoDTO
    {
        
        public int Id { get; set; }
                

        [Required]
        public string Nome { get; set; }
        public string Apelido { get; set; }
        [StringLength(11, ErrorMessage = "CPF tem que conter 11 caracteres", MinimumLength = 11)]
        public string Cpf { get; set; }

        public string Rg { get; set; }

        [Required]
        public String Sexo { get; set; }
        
        public DateTime DataNascimento { get; set; }


        [Required]
        public DateTime DataDeCadastro { get; set; }


        

        
        public UsuarioDTO Usuario { get; set; }

        [Required]
        public FamiliaDTO Familia { get; set; }
    }
}
