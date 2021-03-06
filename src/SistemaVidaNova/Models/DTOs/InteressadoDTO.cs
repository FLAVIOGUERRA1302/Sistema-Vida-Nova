﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class InteressadoDTO
    {
        
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        
        public string Celular { get; set; }
        
        public string Telefone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<EventoDTO> Eventos { get; set; }
    }
}
