using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaVidaNova.Models
{
    public class Voluntario 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        
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
        public bool IsDeletado { get; set; }

        [Required]
        public DateTime DataDeCadastro { get; set; }

        public string Funcao { get; set; }

        public string IdUsuario { get; set; }
        
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        public Endereco Endereco { get; set; }
        public List<Evento> Eventos { get; set; }

        [Required]
        public DateTime DataCurso { get; set; }
        public DateTime? DataAgendamentoCurso { get; set; }

    }
}
