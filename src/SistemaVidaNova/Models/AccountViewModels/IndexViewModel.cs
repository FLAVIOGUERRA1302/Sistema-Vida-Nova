using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.AccountViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAtivo { get; set; }
    }
    public class IndexViewModel
    {
       public List<UserViewModel> lista { get; set; }
    }
}
