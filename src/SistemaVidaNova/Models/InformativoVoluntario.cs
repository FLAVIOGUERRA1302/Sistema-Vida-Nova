﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class InformativoVoluntario
    {
        [Key]
        public int IdVoluntario { get; set; }
        [Key]
        public int IdInformativo { get; set; }

        [ForeignKey("IdVoluntario")]
        public Voluntario Voluntario { get; set; }

        [ForeignKey("IdInformativo")]
        public Informativo Informativo { get; set; }
    }
}
