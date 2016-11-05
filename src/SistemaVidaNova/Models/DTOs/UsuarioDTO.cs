using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class UsuarioDTO
    {

        public string Id { get; set; }

        [Required(ErrorMessage ="Campo obrigatório")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required]
        [Display(Name = "Administrador")]
        public bool IsAdmin { get; set; }

        [Required]
        [Display(Name = "Ativo")]
        public bool IsAtivo { get; set; }



    }
}
