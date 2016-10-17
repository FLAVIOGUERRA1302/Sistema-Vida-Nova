using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class ModeloDeReceitaPlanejamentoDTO
    {

        
        public int Id { get; set; }                        
        
        public double Quantidade { get; set; }
    }
}
