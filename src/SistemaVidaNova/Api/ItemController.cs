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
    public class ItemController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public ItemController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<ItemDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string destino,[FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<Item> query = _context.Item                
                .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            if (!String.IsNullOrEmpty(destino))
            {
                destino = destino.ToUpper();
                query = query.Where(q => q.Destino == destino);

            }



            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<ItemDTO> interessados = query
                .Skip(skip.Value)
                .Take(take.Value).Select(v => new ItemDTO
                {
                    Id = v.Id,                    
                    Nome = v.Nome,
                     Destino = v.Destino,
                      UnidadeDeMedida = v.UnidadeDeMedida
                      
                    
                }).ToList();

              return interessados;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Item q = _context.Item.SingleOrDefault(i => i.Id == id);
            if (q == null)
                return new NotFoundResult();

            ItemDTO dto = new ItemDTO
            {
                Id = q.Id,

                Nome = q.Nome,
                Destino = q.Destino,
                UnidadeDeMedida = q.UnidadeDeMedida
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ItemDTO dto)
        {
            if (ModelState.IsValid)
            {
                dto.Nome = dto.Nome.Trim().ToUpper();
                try
                {
                    switch (dto.Destino)
                {
                    case "ASSOCIACAO":
                        ItemAssociacao ia = new ItemAssociacao()
                        {
                            Nome = dto.Nome,
                            UnidadeDeMedida = dto.UnidadeDeMedida

                        };
                        _context.ItemAssociacao.Add(ia);
                        _context.SaveChanges();
                        dto.Id = ia.Id;

                        
                        break;
                    case "FAVORECIDO":
                            ItemFavorecido itf = new ItemFavorecido()
                            {
                                Nome = dto.Nome,
                                UnidadeDeMedida = dto.UnidadeDeMedida

                            };
                            _context.ItemFavorecido.Add(itf);
                            _context.SaveChanges();
                            dto.Id = itf.Id;
                            break;
                    case "SOPA":
                            ItemSopa its = new ItemSopa()
                            {
                                Nome = dto.Nome,
                                UnidadeDeMedida = dto.UnidadeDeMedida

                            };
                            _context.ItemSopa.Add(its);
                            _context.SaveChanges();
                            dto.Id = its.Id;
                            break;
                        default:
                       return new BadRequestResult();


                }
                return new ObjectResult(dto);
                                        
                    
                }
                catch {
                    ModelState.AddModelError("Nome", "Este item já está cadastrado");
                    return new BadRequestObjectResult(ModelState);
                }
                


            }
            
                return new BadRequestObjectResult(ModelState);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ItemDTO dto)
        {
            if (id != dto.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                Item item = _context.Item.Single(q => q.Id == id);


                item.Nome = dto.Nome;
                item.UnidadeDeMedida = dto.UnidadeDeMedida;


                try
                {
                    _context.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("Nome", "Este item já está cadastrado");
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
            Item interessado = _context.Item.Single(q => q.Id == id);
            _context.Item.Remove(interessado);
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
