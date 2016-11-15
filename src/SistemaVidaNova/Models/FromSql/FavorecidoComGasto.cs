using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.FromSql
{
    
    public class FavorecidoComGasto
    {
        [Key]
        public int Id { get; set; }


        public string Nome { get; set; }

        
        public double ValorGasto { get; set; }
    }
}
