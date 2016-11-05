using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{

    public class ResultadoGeralItemDTO
    {

        public string Item { get; set; }
        public double Quantidade { get; set; }
        public string UnidadeDeMedida { get; set; }
    }

    public class ResultadoGeralDTO
    {

        public ResultadoGeralDTO()
        {
            TotalLitrosDeSopa = 0;
            Itens = new List<ResultadoGeralItemDTO>();
        }
        public double TotalLitrosDeSopa { get; set; }
        public List<ResultadoGeralItemDTO> Itens { get; set; }
    }
    
}
