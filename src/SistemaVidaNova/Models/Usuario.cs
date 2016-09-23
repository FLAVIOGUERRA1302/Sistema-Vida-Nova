using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        public string Cpf { get; set; }
        
    }
}
