using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class DoacaoSopaController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public DoacaoSopaController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<DoacaoSopaDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<DoacaoSopa> query = _context.DoacaoSopa.Include(q=>q.Doador).Include(q=>q.Item)                
                .OrderByDescending(q => q.Data);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q =>  q.Descricao.Contains(filtro) );

            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<DoacaoSopaDTO> doacoes = new List<DoacaoSopaDTO>();
            query = query.Skip(skip.Value).Take(take.Value);
            foreach( var v in query)
            {
                doacoes.Add(new DoacaoSopaDTO
                {
                    Id = v.Id,
                    DataDaDoacao = v.Data,
                    Descricao = v.Descricao,
                    Doador = new DoadorDTOR()
                    {
                        Id = v.Doador.CodDoador,
                        NomeRazaoSocial = v.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)v.Doador).Nome : ((PessoaJuridica)v.Doador).RazaoSocial
                    },
                     Quantidade = v.Quantidade,
                      Item =new ItemDTOR()
                      {
                          Id = v.Item.Id,
                          Nome = v.Item.Nome,
                          UnidadeDeMedida = v.Item.UnidadeDeMedida
                      }
                });
            }
           

              return doacoes;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            DoacaoSopa q = _context.DoacaoSopa.Include(d=>d.Doador).Include(d=>d.Item).SingleOrDefault(i => i.Id == id);
            if (q == null)
                return new NotFoundResult();

            DoacaoSopaDTO dto = new DoacaoSopaDTO
            {
                Id = q.Id,
                DataDaDoacao = q.Data,
                Descricao = q.Descricao,
                Doador = new DoadorDTOR()
                {
                    Id = q.Doador.CodDoador,
                    NomeRazaoSocial = q.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)q.Doador).Nome : ((PessoaJuridica)q.Doador).RazaoSocial,
                    Tipo = q.Doador.GetType() == typeof(PessoaFisica)?"PF":"PJ"

                },
                Quantidade = q.Quantidade,
                Item = new ItemDTOR()
                {
                    Id = q.Item.Id,
                    Nome = q.Item.Nome,
                    UnidadeDeMedida = q.Item.UnidadeDeMedida
                }
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]DoacaoSopaDTO dto)
        {
            if (ModelState.IsValid)
            {
                
                Doador doador = _context.Doador.SingleOrDefault(q => q.CodDoador == dto.Doador.Id);
                if(doador == null)
                {
                    ModelState.AddModelError("Doador", "Doador inválido");
                    return new BadRequestObjectResult(ModelState);
                }

                Item item = _context.Item.SingleOrDefault(q => q.Id == dto.Item.Id && q.Destino=="SOPA");
                if (item == null)
                {
                    ModelState.AddModelError("Item", "Item inválido");
                    return new BadRequestObjectResult(ModelState);
                }


                //corrige fuso horario do js
                dto.DataDaDoacao = dto.DataDaDoacao.AddHours(-dto.DataDaDoacao.Hour);
                DoacaoSopa novo = new DoacaoSopa()
                {
                    Doador = doador,
                    Data = dto.DataDaDoacao,
                    Descricao = dto.Descricao,
                    Item = item,
                    Quantidade = dto.Quantidade
                };
                try
                {
                    _context.DoacaoSopa.Add(novo);
                
                    _context.SaveChanges();
                    dto.Id = novo.Id;
                    return new ObjectResult(dto);
                }
                catch {
                    
                    return new BadRequestObjectResult(ModelState);
                }
                


            }
            
                return new BadRequestObjectResult(ModelState);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]DoacaoSopaDTO dto)
        {
            if (id != dto.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                //corrige fuso horario do js
                dto.DataDaDoacao = dto.DataDaDoacao.AddHours(-dto.DataDaDoacao.Hour);
                DoacaoSopa dd = _context.DoacaoSopa.Single(q => q.Id == id);

                Doador doador = _context.Doador.SingleOrDefault(q => q.CodDoador == dto.Doador.Id);
                if (doador == null)
                {
                    ModelState.AddModelError("Doador", "Doador inválido");
                    return new BadRequestObjectResult(ModelState);
                }
                Item item = _context.Item.SingleOrDefault(q => q.Id == dto.Item.Id && q.Destino == "SOPA");
                if (item == null)
                {
                    ModelState.AddModelError("Item", "Item inválido");
                    return new BadRequestObjectResult(ModelState);
                }

                dd.Doador = doador;
                dd.Data = dto.DataDaDoacao;
                dd.Descricao = dto.Descricao;
                dd.Item = item;
                dd.Quantidade = dto.Quantidade;


                try
                {
                    _context.SaveChanges();
                }
                catch
                {
                    
                    return new BadRequestObjectResult(ModelState);
                }

                return new ObjectResult(dto);
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            DoacaoSopa ds = _context.DoacaoSopa.Single(q => q.Id == id);
            _context.DoacaoSopa.Remove(ds);
            try
            {
                _context.SaveChanges();
                return new NoContentResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}
