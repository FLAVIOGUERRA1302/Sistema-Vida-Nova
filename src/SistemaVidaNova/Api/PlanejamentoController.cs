using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class PlanejamentoController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public PlanejamentoController(VidaNovaContext context)
        {
            _context = context;
        }

    

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]List<ModeloDeReceitaPlanejamentoDTO> modelos)
        {
            if (ModelState.IsValid)
            {

                var query = from q in (from mod in modelos
                                       join modelo in _context.ModeloDeReceita on mod.Id equals modelo.Id
                                       join im in _context.ModeloDeReceitaItem on modelo.Id equals im.IdModeloDeReceita
                                       join item in _context.Item on im.Item.Id equals item.Id
                                       select new
                                       {
                                           item = item,
                                           quantidade = mod.Quantidade * im.Quantidade
                                       })
                            group q by q.item into g
                            select new
                            {
                                item = g.Key,
                                quantidadeNecessaria = g.Sum(x=>x.quantidade)
                            };


                ChartDTO chart = new ChartDTO();
                SerieDTO serie = new SerieDTO()
                {
                    Name = "Despesas",
                    Type = "bar",
                    datapoints = new List<IDataPoint>()
                };
                chart.Data.Add(serie);
                foreach (var q in query)
                {
                    double quantidade = Math.Floor(q.item.QuantidadeEmEstoque / q.quantidadeNecessaria);

                    serie.datapoints.Add(new DataPointString()
                    {
                        x = q.item.Nome,
                        y = quantidade
                    });
                }



                return new ObjectResult(chart);

            }            
            return new BadRequestObjectResult(ModelState);
            
        }

        
    }
}
