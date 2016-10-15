﻿using System;
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
    public class ResultadoSopaController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public ResultadoSopaController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<ResultadoSopaDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<ResultadoSopa> query = _context.ResultadoSopa
                .Include(q=>q.Itens)
                .ThenInclude(q=>q.Item)
                .Include(q=>q.ModeloDeReceita)
                .OrderByDescending(q => q.Data);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q =>  q.Descricao.Contains(filtro) || q.ModeloDeReceita.Nome.Contains(filtro) );

            this.Response.Headers.Add("totalItems", query.Count().ToString());
                        
            query = query.Skip(skip.Value).Take(take.Value);

            List<ResultadoSopaDTO> modelos = query.Select(q => new ResultadoSopaDTO()
            {
                 Id = q.Id,
                     ModeloDeReceita = new ModeloDeReceitaDTOR()
                     {
                          Id = q.ModeloDeReceita.Id,
                           Nome = q.ModeloDeReceita.Nome
                     },
                   Descricao = q.Descricao,
                   Itens = q.Itens.Select(i=> new ResultadoSopaItemDTO()
                   {
                        Item = new ItemDTOR()
                        {
                             Id = i.Item.Id,
                              Nome = i.Item.Nome,
                               UnidadeDeMedida = i.Item.UnidadeDeMedida
                        },
                         Quantidade = i.Quantidade
                   }).ToList(),
                    Data = q.Data,
                     LitrosProduzidos = q.LitrosProduzidos
            }).ToList();


            return modelos;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ResultadoSopa q = _context.ResultadoSopa
                .Include(r => r.Itens)
                .ThenInclude(r => r.Item)
                .Include(r => r.ModeloDeReceita)
                .SingleOrDefault(r => r.Id == id);
            if (q == null)
                return new NotFoundResult();

            ResultadoSopaDTO dto = new ResultadoSopaDTO()
            {
                Id = q.Id,
                 ModeloDeReceita = new ModeloDeReceitaDTOR()
                 {
                      Id=q.ModeloDeReceita.Id,
                       Nome = q.ModeloDeReceita.Nome
                 },
                Descricao = q.Descricao,
                Itens = q.Itens.Select(i => new ResultadoSopaItemDTO()
                {
                    Item = new ItemDTOR()
                    {
                        Id = i.Item.Id,
                        Nome = i.Item.Nome,
                        UnidadeDeMedida = i.Item.UnidadeDeMedida
                    },
                    Quantidade = i.Quantidade
                }).ToList(),
                 Data=q.Data,
                  LitrosProduzidos =q.LitrosProduzidos
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ResultadoSopaDTO dto)
        {
            if (ModelState.IsValid)
            {
                //corrige fuso horario do js
                dto.Data = dto.Data.AddHours(-dto.Data.Hour);

                if (dto.Itens.Count ==0)
                {
                    ModelState.AddModelError("Itens", "O modelo precisa ter itens");
                    return new BadRequestObjectResult(ModelState);
                }
                ModeloDeReceita mr = _context.ModeloDeReceita.SingleOrDefault(q => q.Id == dto.ModeloDeReceita.Id);
                if (mr == null)
                {
                    ModelState.AddModelError("ModeloDeReceita", "Modelo de receita inexitente");
                    return new BadRequestObjectResult(ModelState);
                }

                ResultadoSopa resultadoSopa = new ResultadoSopa()
                {
                    ModeloDeReceita = mr,
                    Descricao = dto.Descricao,
                    Data = dto.Data,
                    LitrosProduzidos = dto.LitrosProduzidos,
                    Itens = new List<ResultadoSopaItem>()
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
                    resultadoSopa.Itens.Add(new ResultadoSopaItem
                    {
                        Item = i.item,
                        Quantidade = i.quantidade
                    });
                }




                try
                {
                    _context.ResultadoSopa.Add(resultadoSopa);
                
                    _context.SaveChanges();
                    dto.Id = resultadoSopa.Id;
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
        public IActionResult Put(int id, [FromBody]ResultadoSopaDTO dto)
        {
            if (id != dto.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                //corrige fuso horario do js
                dto.Data = dto.Data.AddHours(-dto.Data.Hour);
                if (dto.Itens.Count == 0)
                {
                    ModelState.AddModelError("Itens", "O modelo precisa ter itens");
                    return new BadRequestObjectResult(ModelState);
                }

                ModeloDeReceita mr = _context.ModeloDeReceita.SingleOrDefault(q => q.Id == dto.ModeloDeReceita.Id);
                if (mr == null)
                {
                    ModelState.AddModelError("ModeloDeReceita", "Modelo de receita inexitente");
                    return new BadRequestObjectResult(ModelState);
                }

                ResultadoSopa resultadoSopa = _context.ResultadoSopa
                    .Include(q => q.Itens)
                    .ThenInclude(q => q.Item)
                    .Include(q=>q.ModeloDeReceita)
                    .SingleOrDefault(q => q.Id == id);

                resultadoSopa.ModeloDeReceita = mr;
                resultadoSopa.Descricao = dto.Descricao;
                resultadoSopa.Data = dto.Data;
                resultadoSopa.LitrosProduzidos = dto.LitrosProduzidos;

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
                List<ResultadoSopaItem> itensCorretos = new List<ResultadoSopaItem>();

                foreach(var i in itensNovos)
                {
                    var existente = resultadoSopa.Itens.SingleOrDefault(q => q.IdItem == i.item.Id);
                    if (existente == null)
                    {

                        ResultadoSopaItem novoItem =new ResultadoSopaItem
                        {
                            Item = i.item,
                            Quantidade = i.quantidade
                        };

                        resultadoSopa.Itens.Add(novoItem);
                        itensCorretos.Add(novoItem);
                    }
                    else
                    {
                        existente.Quantidade = i.quantidade;
                        itensCorretos.Add(existente);
                    }
                }

                //remove os incorretos
                foreach (var item in resultadoSopa.Itens.Except(itensCorretos).ToArray())
                    resultadoSopa.Itens.Remove(item);


            

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
            ResultadoSopa dd = _context.ResultadoSopa.Single(q => q.Id == id);
            _context.ResultadoSopa.Remove(dd);
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
