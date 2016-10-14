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
    public class ModeloDeReceitaController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public ModeloDeReceitaController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<ModeloDeReceitaDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<ModeloDeReceita> query = _context.ModeloDeReceita.Include(q=>q.Itens).ThenInclude(q=>q.Item)                
                .OrderByDescending(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q =>  q.Descricao.Contains(filtro) || q.Nome.Contains(filtro) );

            this.Response.Headers.Add("totalItems", query.Count().ToString());
                        
            query = query.Skip(skip.Value).Take(take.Value);

            List<ModeloDeReceitaDTO> modelos = query.Select(q => new ModeloDeReceitaDTO()
            {
                 Id = q.Id,
                  Nome = q.Nome,
                   Descricao = q.Descricao,
                   Itens = q.Itens.Select(i=> new ModeloDeReceitaItemDTO()
                   {
                        Item = new ItemDTOR()
                        {
                             Id = i.Item.Id,
                              Nome = i.Item.Nome,
                               UnidadeDeMedida = i.Item.UnidadeDeMedida
                        },
                         Quantidade = i.Quantidade
                   }).ToList()
            }).ToList();


            return modelos;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ModeloDeReceita q = _context.ModeloDeReceita.Include(i => i.Itens).ThenInclude(i => i.Item).SingleOrDefault(i => i.Id == id);
            if (q == null)
                return new NotFoundResult();

            ModeloDeReceitaDTO dto = new ModeloDeReceitaDTO()
            {
                Id = q.Id,
                Nome = q.Nome,
                Descricao = q.Descricao,
                Itens = q.Itens.Select(i => new ModeloDeReceitaItemDTO()
                {
                    Item = new ItemDTOR()
                    {
                        Id = i.Item.Id,
                        Nome = i.Item.Nome,
                        UnidadeDeMedida = i.Item.UnidadeDeMedida
                    },
                    Quantidade = i.Quantidade
                }).ToList()
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ModeloDeReceitaDTO dto)
        {
            if (ModelState.IsValid)
            {
                if(dto.Itens.Count ==0)
                {
                    ModelState.AddModelError("Itens", "O modelo precisa ter itens");
                    return new BadRequestObjectResult(ModelState);
                }

                ModeloDeReceita modelo = new ModeloDeReceita()
                {
                    Nome = dto.Nome,
                    Descricao = dto.Descricao,
                    Itens = new List<ModeloDeReceitaItem>()
               };


                var itensNovos = from i in _context.Item
                                 join d in dto.Itens on i.Id equals d.Item.Id
                                 where i.Destino == "SOPA"
                                 select new
                                 {
                                     item = i,
                                     quantidade = d.Quantidade
                                 };
                if (itensNovos.Count() != dto.Itens.Count)
                {
                    ModelState.AddModelError("Itens", "A lista de itens contém itens inválidos");
                    return new BadRequestObjectResult(ModelState);
                }

                foreach (var i in itensNovos)
                {
                    modelo.Itens.Add(new ModeloDeReceitaItem
                    {
                        Item = i.item,
                        Quantidade = i.quantidade
                    });
                }




                try
                {
                    _context.ModeloDeReceita.Add(modelo);
                
                    _context.SaveChanges();
                    dto.Id = modelo.Id;
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
        public IActionResult Put(int id, [FromBody]ModeloDeReceitaDTO dto)
        {
            if (id != dto.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {

                if (dto.Itens.Count == 0)
                {
                    ModelState.AddModelError("Itens", "O modelo precisa ter itens");
                    return new BadRequestObjectResult(ModelState);
                }
                
                ModeloDeReceita modelo = _context.ModeloDeReceita.Include(q=>q.Itens).ThenInclude(q=>q.Item).SingleOrDefault(q => q.Id == id);

                modelo.Nome = dto.Nome;
                modelo.Descricao = dto.Descricao;

                var itensNovos = from i in _context.Item
                            join d in dto.Itens on i.Id equals d.Item.Id
                            where i.Destino == "SOPA"
                            select new
                            {
                                item = i,
                                quantidade = d.Quantidade
                            };
                if(itensNovos.Count()!= dto.Itens.Count)
                {
                    ModelState.AddModelError("Itens", "A lista de itens contém itens inválidos");
                    return new BadRequestObjectResult(ModelState);
                }
                List<ModeloDeReceitaItem> itensCorretos = new List<ModeloDeReceitaItem>();

                foreach(var i in itensNovos)
                {
                    var existente = modelo.Itens.SingleOrDefault(q => q.IdItem == i.item.Id);
                    if (existente == null)
                    {

                        ModeloDeReceitaItem novoItem =new ModeloDeReceitaItem
                        {
                            Item = i.item,
                            Quantidade = i.quantidade
                        };

                        modelo.Itens.Add(novoItem);
                        itensCorretos.Add(novoItem);
                    }
                    else
                    {
                        existente.Quantidade = i.quantidade;
                        itensCorretos.Add(existente);
                    }
                }

                //remove os incorretos
                foreach (var item in modelo.Itens.Except(itensCorretos).ToArray())
                    modelo.Itens.Remove(item);


            

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
            ModeloDeReceita dd = _context.ModeloDeReceita.Single(q => q.Id == id);
            _context.ModeloDeReceita.Remove(dd);
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
