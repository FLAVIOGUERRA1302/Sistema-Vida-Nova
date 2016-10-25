using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SistemaVidaNova.Models.DTOs
{
    public class VoluntarioDTO
    {
       
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }        
        [Required]
        [StringLength(200, ErrorMessage = "O {0} tem que ter no máximo {1} caracteres")]
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
        [StringLength(200)]
        public string Funcao { get; set; }
        public Usuario usuario { get; set; }

        public DateTime DataDeCadastro { get; set; }
        [Required]
        public EnderecoDTO Endereco { get; set; }
        public List<EventoDTO> Eventos { get; set; }
                
        public DateTime DataCurso { get; set; }
        public DateTime? DataAgendamentoCurso { get; set; }
        public int DiasEmAtraso { get; set; }
    }
}
