using SistemaVidaNova.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class DespesaDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }
        [Required]
        [StringLength(10)]
        public string Tipo { get; set; }

        [Required]
        public ItemDTOR Item { get; set; }

        [Required]
        public DateTime DataDaCompra { get; set; }

        [Required]
        public Double Quantidade { get; set; }

        [Required]
        public Double ValorUnitario { get; set; }
        public FavorecidoDTOR Favorecido { get; set; }

        public UsuarioDTO Usuario { get; set; }

    }
}
