using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class ConhecimentoProficional
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }

        [Required]
        public string CodFavorecido { get; set; }

        [ForeignKey("CodFavorecido")]
        public Favorecido Favorecido { get; set; }
    }
}
