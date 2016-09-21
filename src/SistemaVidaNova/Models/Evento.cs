using System;
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
        public double ValorDeEntrada { get; set; }

        [Required]
        public double ValorArrecadado { get; set; }
                
        public string Relato { get; set; }

        public string VoluntarioId { get; set; }

        [ForeignKey("VoluntarioId")]
        public Voluntario Voluntario { get; set; }

    }
}
