using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.FromSql
{
    
    public class EventoMaisProcurado
    {
        [Key]
        public int CodEvento { get; set; }
        
        public string Titulo { get; set; }
        
        public string Descricao { get; set; }
        
        public string Cor { get; set; }
        
        public string CorDaFonte { get; set; }
        
        public DateTime DataInicio { get; set; }
        
        public DateTime DataFim { get; set; }


        public double ValorArrecadado { get; set; }

        public string Relato { get; set; }

        public int QuantidadeDePessoas { get; set; }
    }
}
