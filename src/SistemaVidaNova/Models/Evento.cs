﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class Evento
    {
        [Key]
        public int CodEvento { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public string Cor { get; set; }
        [Required]
        public string CorDaFonte { get; set; }
        [Required]
        public DateTime DataInicio { get; set; }
        [Required]
        public DateTime DataFim { get; set; }
        

        [Required]
        public double ValorArrecadado { get; set; }
                
        public string Relato { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        public List<VoluntarioEvento> Voluntarios { get; set; }
        public List<InteressadoEvento> Interessados { get; set; }

        public List<DoadorEvento> Doadores { get; set; }

        public List<FavorecidoEvento> Favorecidos { get; set; }


    }
}
