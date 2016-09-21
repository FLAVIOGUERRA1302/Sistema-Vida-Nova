﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class InteressadoEvento
    {
        [Key]
        public int CodInetessado { get; set; }
        [Key]
        public int CodEvento { get; set; }

        [ForeignKey("CodInteressado")]
        public Interessado Interessado { get; set; }

        [ForeignKey("CodEvento")]
        public Evento Evento { get; set; }
    }
}
