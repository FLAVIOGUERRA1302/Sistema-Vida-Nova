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
            ChartData = new SimpleCharDataDTO<DateTime>();
        }
        public double TotalLitrosDeSopa { get; set; }
        public List<ResultadoGeralItemDTO> Itens { get; set; }

        public SimpleCharDataDTO<DateTime> ChartData { get; set; }
    }
    
}
