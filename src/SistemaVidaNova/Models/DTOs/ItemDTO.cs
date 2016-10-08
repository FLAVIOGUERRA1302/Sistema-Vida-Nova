using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class ItemDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        [Required]
        [StringLength(10)]
        public string Destino { get; set; }
        [Required]
        [StringLength(4)]
        public string UnidadeDeMedida { get; set; }
    }
}
