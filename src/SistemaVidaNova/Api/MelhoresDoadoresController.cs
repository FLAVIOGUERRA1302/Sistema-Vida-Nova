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
    public class MelhoresDoadoresController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public MelhoresDoadoresController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<DoadorComQuantidadeDeDoacoes> Get([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            List<DoadorComQuantidadeDeDoacoes> melhores = new List<DoadorComQuantidadeDeDoacoes>();

            if (start == null || end == null)
                return melhores;

            melhores = _context.MelhorDoador
                .FromSql<DoadorComQuantidadeDeDoacoes>(@"select top 10 doador.CodDoador as Id, CONCAT(doador.Nome,doador.RazaoSocial) as NomeRazaoSocial, doador.doador_type as Tipo, CONCAT(doador.Cpf,doador.Cnpj) as CpfCnpj, QuantidadeDeDoacoes
	                                    from Doador as doador inner join
	                                    (
	                                    select CodDoador, count(*) as QuantidadeDeDoacoes
	                                    from (select CodDoador
	                                    from DoacaoObjeto
	                                    where DataDaDoacao between {0} and {1}
	                                    union all
	                                    select CodDoador
	                                    from DoacaoSopa
	                                    where Data between {0} and {1}
	                                    ) as u 
	                                    group by CodDoador
	                                    ) as doacoes on doador.CodDoador = doacoes.CodDoador
	
	                                    order by doacoes.QuantidadeDeDoacoes desc, doador.Nome, doador.RazaoSocial  "
                                        , start.Value,end.Value)
                                                                                                .AsNoTracking()
                                                                                                .ToList();


         
         

            return melhores;
        }

      

       
    }
}
