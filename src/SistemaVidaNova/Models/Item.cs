using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{

    public class Item
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        [Required]
        [StringLength(10)]
        public string Destino { get; set; }
        [Required]
        [StringLength(10)]
        public string UnidadeDeMedida { get; set; }

        [Required]
        public double QuantidadeEmEstoque { get; set; }

    }

    public class ItemAssociacao : Item
    {
        
    }

    public class ItemFavorecido : Item
    {
        [Required]
        public int CodFavorecido { get; set; }

        [ForeignKey("CodFavorecido")]        
        Favorecido Favorecido { get; set; }
    }

    public class ItemSopa : Item
    {

    }
}
