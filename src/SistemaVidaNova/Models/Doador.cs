using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    
    public class Doador
    {

        [Key]
        public int CodDoador { get; set; }

        
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }

        [Required]
        public Endereco Endereco { get; set; }
    }

    public class PessoaFisica : Doador
    {
        [MaxLength(200)]
        public string Nome { get; set; }
        [StringLength(11, ErrorMessage = "CPF tem que conter 11 caracteres", MinimumLength = 11)]
        public string Cpf { get; set; }
    }

    public class PessoaJuridica : Doador
    {
        [MaxLength(500)]
        public string RazaoSocial { get; set; }
        [StringLength(14, ErrorMessage = "CNPJ tem que conter 14 caracteres", MinimumLength = 14)]
        public string Cnpj { get; set; }
    }
}
