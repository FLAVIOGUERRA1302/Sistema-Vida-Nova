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
    public class FavorecidosComMaisGastoController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public FavorecidosComMaisGastoController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<FavorecidoComGasto> Get([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            List<FavorecidoComGasto> favorecidos = new List<FavorecidoComGasto>();

            if (start == null || end == null)
                return favorecidos;

            favorecidos = _context.FavorecidoComGasto
                .FromSql<FavorecidoComGasto>(@"SELECT f.CodFavorecido as Id, f.Nome , sum(Quantidade*  ValorUnitario) as ValorGasto    
                                        FROM Item AS i INNER JOIN
                                            Despesa AS d ON i.Id = d.IdItem INNER JOIN
                                            Favorecido as f ON d.CodFavorecido = f.CodFavorecido
                                        where i.Destino = 'FAVORECIDO' and
                                        d.DataDaCompra between {0} and {1}
                                        group by f.CodFavorecido, f.Nome
                                        order by sum(Quantidade*  ValorUnitario) desc"
                                        , start.Value,end.Value)
                                        .AsNoTracking()
                                        .ToList();


         
         

            return favorecidos;
        }

      

       
    }
}
