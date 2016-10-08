using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class DoadorDTO
    {
       
        
        public int Id { get; set; }

        [Required]
        public string Tipo  { get; set; }
        [Required]
        public string NomeRazaoSocial { get; set; }
        [Required]
        public string CpfCnpj { get; set; }

        public string Celular { get; set; }
        
        public string Telefone { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public EnderecoDTO Endereco { get; set; }
    }
}
