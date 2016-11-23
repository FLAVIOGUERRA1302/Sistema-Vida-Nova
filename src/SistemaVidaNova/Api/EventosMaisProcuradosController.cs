using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using SistemaVidaNova.Models.FromSql;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class EventosMaisProcuradosController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public EventosMaisProcuradosController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<EventoMaisProcurado> Get([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            List<EventoMaisProcurado> maisProcurados = new List<EventoMaisProcurado>();

            if (start == null || end == null)
                return maisProcurados;

            maisProcurados = _context.EventoMaisProcurado
                .FromSql<EventoMaisProcurado>(@"select top 10 evento.CodEvento, Titulo, Descricao, Cor, CorDaFonte, DataInicio, DataFim, ValorArrecadado, ISNULL(Relato, '' ) as Relato, pessoas.QuantidadeDePessoas
                                                from Evento as evento inner join
                                                (
                                                select CodEvento, count(*)-1 as QuantidadeDePessoas
                                                from (select CodEvento
                                                from VoluntarioEvento
                                                union all
                                                select CodEvento
                                                from InteressadoEvento
												union all
                                                select CodEvento
                                                from FavorecidoEvento
												union all
                                                select CodEvento
                                                from DoadorEvento
                                                union all
                                                select CodEvento
                                                from Evento
                                                ) as u 
                                                group by CodEvento
                                                ) as pessoas on evento.CodEvento = pessoas .CodEvento
                                                where DataInicio between {0} and {1}
                                                order by pessoas.QuantidadeDePessoas desc", start.Value,end.Value)
                                                                                                .AsNoTracking()
                                                                                                .ToList();


         
         

            return maisProcurados;
        }

      

       
    }
}
