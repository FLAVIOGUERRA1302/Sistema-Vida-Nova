using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.AccountViewModels
{
    public class UserViewModel
    {
        
        public string Id { get; set; }
        [Required(ErrorMessage ="O Nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }
        
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Administrador")]
        public bool IsAdmin { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Ativo")]
        public bool IsAtivo { get; set; }
    }
}
