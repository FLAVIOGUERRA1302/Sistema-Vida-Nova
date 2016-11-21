using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;
using Microsoft.AspNetCore.Http;
using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http;

using SistemaVidaNova.Services;
using MimeKit;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Authorize]    
    [Route("api/[controller]")]
    public class ChartController : Controller
    {
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        
        public ChartController(
            VidaNovaContext context,
            UserManager<Usuario> userManager
        )
        {
            _context = context;
            _userManager = userManager;
            
        }

        // GET: api/values
        [HttpGet("{grafico}")]
        public IActionResult Get(string grafico, [FromQuery]DateTime? start, [FromQuery]DateTime? end, string filtro,int? id)
        {
            switch (grafico.ToUpper())
            {
                case "VOLUNTARIODIADASEMANA":
                        return new ObjectResult(VoluntarioDiaDaSemana());
                case "DESPESAPORITEMNOPERIODO":
                    return new ObjectResult(DespesaPorItemNoPeriodo(start.Value,end.Value,filtro));
                case "DOACOESMENSAISNOPERIODO":
                    return new ObjectResult(DoacoesMensaisNoPeriodo(start.Value, end.Value));
                case "DOACOESMENSAISPORDOADORNOPERIODO":
                    return new ObjectResult(DoacoesMensaisPorDoadorNoPeriodo(start.Value, end.Value,id.Value));

            }

            return new NoContentResult();
        }
        /*
        // GET: api/values
        [HttpGet("pdf/{grafico}")]
        
        public IActionResult pdf(string grafico, [FromQuery]DateTime? start, [FromQuery]DateTime? end, string filtro, int? id)
        {
            PdfDocument document = null;
            string nome = "";
            switch (grafico.ToUpper())
            {
                case "VOLUNTARIODIADASEMANA":
                    document = PfdVoluntarioDiaDaSemana();
                        break;
                case "DESPESAPORITEMNOPERIODO":
                    document = PfdDespesaPorItemNoPeriodo(start.Value, end.Value, filtro);
                    break;
                case "DOACOESMENSAISNOPERIODO":
                    document = PfdDoacoesMensaisNoPeriodo(start.Value, end.Value);
                    break;
                case "DOACOESMENSAISPORDOADORNOPERIODO":
                    document = PfdDoacoesMensaisPorDoadorNoPeriodo(start.Value, end.Value, id.Value);
                    break;

            }

            if (document != null) {
                MemoryStream ms = new MemoryStream();
                document.Save(ms);
                //If the position is not set to '0' then the PDF will be empty.
                ms.Position = 0;

                //Download the PDF document in the browser.
                FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");
                fileStreamResult.FileDownloadName = nome;
                return fileStreamResult;
            }
            return new NoContentResult();

            
        }

        private PdfDocument PfdDoacoesMensaisPorDoadorNoPeriodo(DateTime value1, DateTime value2, int value3)
        {
            throw new NotImplementedException();
        }

        private PdfDocument PfdDoacoesMensaisNoPeriodo(DateTime value1, DateTime value2)
        {
            throw new NotImplementedException();
        }

        private PdfDocument PfdDespesaPorItemNoPeriodo(DateTime start, DateTime end, string filtro)
        {
            PdfDocument document;
            PdfPage page;
            Color gray = Color.FromArgb(255, 77, 77, 77);
            Color black = Color.FromArgb(255, 0, 0, 0);
            Color white = Color.FromArgb(255, 255, 255, 255);
            Color violet = Color.FromArgb(255, 151, 108, 174);

            var query = from q in _context.Despesa
                        where q.Tipo == filtro
                        && q.DataDaCompra >= start && q.DataDaCompra <= end
                        group q by q.Item into g
                        orderby g.Key.Nome
                        select new
                        {
                            item = g.Key,
                            valorTotal = g.Sum(v => v.Quantidade * v.ValorUnitario)
                        };

            document = new PdfDocument();
            

            //Setting margin
            document.PageSettings.Margins.All = 0;
            //Adding a new page
            page = document.Pages.Add();
            PdfGraphics g = page.Graphics;

            //Creating font instances
            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 35);
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 16);











            return document;

        }

        private PdfDocument PfdVoluntarioDiaDaSemana()
        {
            throw new NotImplementedException();
        }
        */
        private ChartDTO DespesaPorItemNoPeriodo(DateTime start, DateTime end, string filtro)
        {
            ChartDTO chart = new ChartDTO();
            SerieDTO serie = new SerieDTO()
            {
                Name = "Despesas",
                Type = "bar",
                datapoints = new List<IDataPoint>()
            };
            chart.Data.Add(serie);

            var query = from q in _context.Despesa
                        where q.Tipo == filtro
                        && q.DataDaCompra >= start && q.DataDaCompra <= end
                        group q by q.Item.Nome into g
                        orderby g.Key
                        select new
                        {
                            item = g.Key,
                            valorTotal = g.Sum(v => v.Quantidade * v.ValorUnitario)
                        };

            foreach(var q in query)
            {
                serie.datapoints.Add(new DataPointString()
                {
                    x = q.item,
                    y = q.valorTotal
                });
            }
            return chart;
        }

        private ChartDTO VoluntarioDiaDaSemana()
        {
            ChartDTO chart = new ChartDTO();
            SerieDTO serie = new SerieDTO()
            {
                Name = "Dias da Semana",
                Type = "pie",
                datapoints = new List<IDataPoint>()
            };
            chart.Data.Add(serie);
            if (_context.Voluntario.Any())
            {
                serie.datapoints.Add(new DataPointString()
                {
                    x = "Domingo",
                    y = _context.Voluntario.Where(q => q.Domingo == true && q.IsDeletado ==false).Count()
                });

                serie.datapoints.Add(new DataPointString()
                {
                    x = "Segunda-feira",
                    y = _context.Voluntario.Where(q => q.SegundaFeira == true && q.IsDeletado == false).Count()
                });

                serie.datapoints.Add(new DataPointString()
                {
                    x = "Terça-feira",
                    y = _context.Voluntario.Where(q => q.TercaFeira == true && q.IsDeletado == false).Count()
                });

                serie.datapoints.Add(new DataPointString()
                {
                    x = "Quarta-feira",
                    y = _context.Voluntario.Where(q => q.QuartaFeira == true && q.IsDeletado == false).Count()
                });

                serie.datapoints.Add(new DataPointString()
                {
                    x = "Quinta-feira",
                    y = _context.Voluntario.Where(q => q.QuintaFeira == true && q.IsDeletado == false).Count()
                });

                serie.datapoints.Add(new DataPointString()
                {
                    x = "Sexta-feira",
                    y = _context.Voluntario.Where(q => q.SextaFeira == true && q.IsDeletado == false).Count()
                });

                serie.datapoints.Add(new DataPointString()
                {
                    x = "Sábado",
                    y = _context.Voluntario.Where(q => q.Sabado == true && q.IsDeletado == false).Count()
                });


            }

            return chart;
        }


        private SimpleCharDataDTO<DateTime> DoacoesMensaisNoPeriodo(DateTime start, DateTime end)
        {
            SimpleCharDataDTO<DateTime> chart = new SimpleCharDataDTO<DateTime>();
            List<double> serie = new List<double>();
            chart.Series.Add(serie);
            
                    var queryD = from q in (from dd in _context.DoacaoDinheiro
                                            where dd.Data >= start && dd.Data <= end
                                            select new
                                            {
                                                //mes = new DateTime(dd.Data.Year, dd.Data.Month, 1),
                                                mes = dd.Data.Month,
                                                ano = dd.Data.Year,
                                                valor = dd.Valor
                                            })
                                 group q by new { q.ano, q.mes } into g
                                 orderby g.Key.ano, g.Key.mes
                                 select new
                                 {
                                     mes = g.Key.mes,
                                     ano = g.Key.ano,
                                     valor = g.Sum(v => v.valor)
                                 };
                    foreach(var qd in queryD)
                    {
                            chart.Labels.Add(new DateTime(qd.ano,qd.mes,1));
                        serie.Add(qd.valor);
                    }

                   
            



            return chart;
        }

        private SimpleCharDataDTO<DateTime> DoacoesMensaisPorDoadorNoPeriodo(DateTime start, DateTime end, int id)
        {
            SimpleCharDataDTO<DateTime> chart = new SimpleCharDataDTO<DateTime>();
            List<double> serie = new List<double>();
            chart.Series.Add(serie);

            var queryD = from q in (from dd in _context.DoacaoDinheiro
                                    where dd.Data >= start && dd.Data <= end && dd.CodDoador == id
                                    select new
                                    {
                                        mes = dd.Data.Month,
                                        ano = dd.Data.Year,
                                        valor = dd.Valor
                                    })
                         group q by new { q.ano, q.mes } into g
                         orderby g.Key.ano, g.Key.mes
                         select new
                         {
                             mes = g.Key.mes,
                             ano = g.Key.ano,
                             valor = g.Sum(v => v.valor)
                         };
            foreach (var qd in queryD)
            {
                chart.Labels.Add(new DateTime(qd.ano, qd.mes, 1));
                serie.Add(qd.valor);
            }






            return chart;
        }
    }
}
