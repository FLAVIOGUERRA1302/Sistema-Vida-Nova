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
        
        public DateTime? DataNascimento { get; set; }


        [Required]
        public DateTime DataDeCadastro { get; set; }


        

        
        public UsuarioDTO Usuario { get; set; }

        [MaxLength(200)]
        public string NomeFamilia { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }



        public string Cep { get; set; }
        
        public string Logradouro { get; set; }
        
        public string Bairro { get; set; }
        
        public string Cidade { get; set; }
        
        public string Estado { get; set; }
        
        public string Numero { get; set; }

        public string Complemento { get; set; }

        public List<ConhecimentoProficionalDTO> ConhecimentosProfissionais = new List<ConhecimentoProficionalDTO>();
    }
}
