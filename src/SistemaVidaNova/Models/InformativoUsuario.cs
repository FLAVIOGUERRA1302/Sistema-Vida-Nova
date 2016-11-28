using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class InformativoUsuario
    {
        
        public string IdUsuario { get; set; }
       
        public int IdInformativo { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        [ForeignKey("IdInformativo")]
        public Informativo Informativo { get; set; }
    }
}
