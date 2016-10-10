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
        [HttpGet("{id}")]
        public IActionResult Get(string id, [FromQuery]DateTime? start, [FromQuery]DateTime? end, string filtro)
        {
            switch (id.ToUpper())
            {
                case "VOLUNTARIODIADASEMANA":
                        return new ObjectResult(VoluntarioDiaDaSemana());
                case "DESPESAPORITEMNOPERIODO":
                    return new ObjectResult(DespesaPorItemNoPeriodo(start.Value,end.Value,filtro));

            }

            return new NoContentResult();
        }

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
                        group q by q.Item into g
                        orderby g.Key.Nome
                        select new
                        {
                            item = g.Key,
                            valorTotal = g.Sum(v => v.Quantidade * v.ValorUnitario)
                        };

            foreach(var q in query)
            {
                serie.datapoints.Add(new DataPointString()
                {
                    x = q.item.Nome,
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

    }
}
