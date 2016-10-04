using SistemaVidaNova.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class InformativoDTO
    {
        public int Id { get; set; }
        
        public string Subject { get; set; }
        
        public string Body { get; set; }
        

        public string IdUsuario { get; set; }

        
        public UsuarioDTO Usuario { get; set; }

        public List<VoluntarioDTO> Voluntarios { get; set; }
        public List<DoadorDTO> DoadoresFisicos { get; set; }
        public List<DoadorDTO> DoadoresJuridicos { get; set; }
        public List<UsuarioDTO> Usuarios { get; set; }

        public List<AttachmentDTO> Attachments { get; set; }
    }
}
