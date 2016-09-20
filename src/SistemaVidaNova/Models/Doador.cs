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
        [Required]
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
    }

    public class PessoaFisica : Doador
    {
        [Required]
        public string Cpf { get; set; }
        [Required]
        public string Nome { get; set; }
    }

    public class PessoaJuridica : Doador
    {
        [Required]
        public string Cnpj { get; set; }
        [Required]
        public string RazaoSocial { get; set; }
    }
}
