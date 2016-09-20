using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SistemaVidaNova.Models
{
    public class Voluntario : IdentityUser
    {
       /* [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]*/    
        public string Senha { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cpf { get; set; }
        [Required]
        public string Rg { get; set; }
        public string Celular { get; set; }
        public string Telefone { get; set; }
        [Required]
        public String Sexo { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        public bool SegundaFeira { get; set; }
        [Required]
        public bool TercaFeira { get; set; }
        [Required]
        public bool QuartaFeira { get; set; }
        [Required]
        public bool QuintaFeira { get; set; }
        [Required]
        public bool SextaFeira { get; set; }
        [Required]
        public bool Sabado { get; set; }
        [Required]
        public bool Domingo { get; set; }

        [Required]
        public DateTime DataDeCadastro { get; set; }
        public List<Endereco> Enderecos { get; set; }
    }
}
