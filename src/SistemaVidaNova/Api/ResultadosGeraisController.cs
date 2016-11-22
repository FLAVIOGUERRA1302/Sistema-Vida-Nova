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
                            where df.DataDaCompra >= start.Value && df.DataDaCompra<= end.Value
                            group df by item into g
                            select new ResultadoGeralItemDTO
                            {
                                Item = g.Key.Nome,
                                UnidadeDeMedida = g.Key.UnidadeDeMedida,
                                Quantidade = g.Sum(q => q.Quantidade)
                            }).ToList();

            }
            catch { }



            var queryDoacoes = (from dd in _context.DoacaoDinheiro
                                    where dd.Data >= start && dd.Data <= end
                                    select new
                                    {
                                        //mes = new DateTime(dd.Data.Year, dd.Data.Month, 1),
                                        mes = dd.Data.Month,
                                        ano = dd.Data.Year,
                                        doado = dd.Valor,
                                        despesa = 0.0d

                                    }).ToList();

            var queryDespesas = (from dd in _context.Despesa
                                where dd.DataDaCompra >= start && dd.DataDaCompra <= end
                                select new
                                {
                                    //mes = new DateTime(dd.Data.Year, dd.Data.Month, 1),
                                    mes = dd.DataDaCompra.Month,
                                    ano = dd.DataDaCompra.Year,
                                    doado = 0.0d,
                                    despesa = dd.Quantidade * dd.ValorUnitario
                                }).ToList();
            var query = from q in (queryDoacoes.Concat(queryDespesas))
                               group q by new { q.ano, q.mes } into g
                               orderby g.Key.ano, g.Key.mes
                               select new
                               {
                                   mes = g.Key.mes,
                                   ano = g.Key.ano,
                                   doado = g.Sum(v => v.doado),
                                   despesa = g.Sum(v => v.despesa)
                               };


            //obtem todos os meses primeiro porque pode ter doação em um determinado mês sem despesa e vice versa
           // List<DateTime> meses = queryDoacoes.Select(q => new DateTime(q.ano, q.mes, 1)).Union(queryDespesas.Select(q => new DateTime(q.ano, q.mes, 1))).OrderBy(q=>q).ToList();

            List<double> serieDoacao = new List<double>();
            List<double> serieDespesa = new List<double>();
            resultado.ChartData.Series.Add(serieDoacao);
            resultado.ChartData.Series.Add(serieDespesa);
            resultado.ChartData.SeriesName.Add("Doações");
            resultado.ChartData.SeriesName.Add("Despesas");

           // Dictionary<DateTime, double> dicHelpDoacoes = new Dictionary<DateTime, double>();
           // Dictionary<DateTime, double> dicHelpDespesa = new Dictionary<DateTime, double>();

            foreach (var q in query)
            {
                resultado.ChartData.Labels.Add(new DateTime(q.ano, q.mes, 1));
                serieDoacao.Add(q.doado);
                serieDespesa.Add(q.despesa);

            }

            return new ObjectResult(resultado);

        }

       

        
    }
}
