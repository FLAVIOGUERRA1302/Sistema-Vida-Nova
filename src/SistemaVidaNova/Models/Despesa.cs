using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{

    public class Despesa
    {

        [Key]
        public int Id { get; set; }
                
        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }
        [Required]
        [StringLength(10)]
        public string Tipo { get; set; }

        [Required]
        public int IdItem { get; set; }

        [ForeignKey("IdItem")]
        public Item Item { get; set; }

        [Required]
        public DateTime DataDaCompra { get; set; }

        [Required]
        public Double Quantidade { get; set; }

        [Required]
        public Double ValorUnitario { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
    }

    public class DespesaAssociacao : Despesa
    {
      /*  [Required]
        public new int IdItem { get; set; }

        [ForeignKey("IdItem")]
        public new ItemAssociacao Item { get; set; }*/
    }

    public class DespesaFavorecido : Despesa
    {
       /* [Required]
        public new int IdItem { get; set; }

        [ForeignKey("IdItem")]
        public new ItemFavorecido Item { get; set; }*/

        [Required]
        public int CodFavorecido { get; set; }

        [ForeignKey("CodFavorecido")]        
        public Favorecido Favorecido { get; set; }
    }

    public class DespesaSopa : Despesa
    {
       /* [Required]
        public new int IdItem { get; set; }

        [ForeignKey("IdItem")]
        public new  ItemSopa Item { get; set; }*/
    }
}
