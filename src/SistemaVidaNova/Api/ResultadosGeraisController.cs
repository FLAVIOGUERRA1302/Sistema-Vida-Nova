using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Syncfusion.XlsIO;
using CustomExtensions;
using Syncfusion.Drawing;
using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class ResultadosGerais : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public ResultadosGerais(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]DateTime? start, [FromQuery]DateTime? end)
        {
            
            if (start == null || end == null)
                return new BadRequestResult();

            ResultadoGeralDTO resultado = new ResultadoGeralDTO();

            try
            {
                resultado.TotalLitrosDeSopa = _context.ResultadoSopa.Where(q => q.Data >= start.Value && q.Data <= end.Value).Sum(q => q.LitrosProduzidos);
            }
            catch { }

            try
            {
                resultado.Itens = (from df in _context.DespesaFavorecido
                            join item in _context.Item on df.IdItem equals item.Id
                            group df by item into g
                            select new ResultadoGeralItemDTO
                            {
                                Item = g.Key.Nome,
                                UnidadeDeMedida = g.Key.UnidadeDeMedida,
                                Quantidade = g.Sum(q => q.Quantidade)
                            }).ToList();

            }
            catch { }
           

            return new ObjectResult(resultado);

        }

       

        
    }
}
