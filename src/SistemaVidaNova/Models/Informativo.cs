using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class Informativo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }

        

        public string IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
        
        public List<InformativoVoluntario> Voluntarios { get; set; }
        public List<InformativoDoador> Doadores { get; set; }
        public List<InformativoUsuario> Usuarios { get; set; }

        public List<Attachment> Attachments { get; set; }

    }
}
