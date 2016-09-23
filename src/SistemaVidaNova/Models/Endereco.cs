using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class Endereco
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Cep { get; set; }
        [Required]
        public string Logradouro { get; set; }
        [Required]
        public string Bairro { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public string Numero { get; set; }
        
        public string Complemento { get; set; }

        
        public int VoluntarioId { get; set; }
        
        [ForeignKey("VoluntarioId")]
        public Voluntario Voluntario { get; set; }
    }
}
