using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SistemaVidaNova.Models.DTOs
{
    public class VoluntarioDTOR
    {
        [Required]
        public int Id { get; set; }
                
        [EmailAddress]
        public string Email { get; set; }        
        
        public string Nome { get; set; }
        
        public string Cpf { get; set; }
        
    }
}
