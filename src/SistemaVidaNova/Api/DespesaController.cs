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


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Authorize]    
    [Route("api/[controller]")]
    public class DespesaController : Controller
    {
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        
        public DespesaController(
            VidaNovaContext context,
            UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
            
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<DespesaDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro, [FromQuery]string tipo)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;
            if (String.IsNullOrEmpty(tipo))
                tipo = "ASSOCIACAO"; // pessoa fisica

            List<DespesaDTO> despesas = new List<DespesaDTO>();
            switch (tipo)
            {
                case "ASSOCIACAO":
                    IQueryable<DespesaAssociacao> query = _context.DespesaAssociacao
                        .Include(q => q.Item)
                        .Include(q=>q.Usuario)
                        .OrderByDescending(q => q.DataDaCompra);

                    if (!String.IsNullOrEmpty(filtro))
                        query = query.Where(q => q.Descricao.Contains(filtro) || q.Item.Nome.Contains(filtro));
                    this.Response.Headers.Add("totalItems", query.Count().ToString());

                    despesas = query
                   .Skip(skip.Value)
                   .Take(take.Value).Select(q => new DespesaDTO
                   {
                       Id = q.Id,
                       Descricao = q.Descricao,
                       DataDaCompra = q.DataDaCompra,
                       Quantidade = q.Quantidade,
                       Tipo = q.Tipo,
                       ValorUnitario = q.ValorUnitario,
                        Item = new ItemDespesaDTO()
                        {
                              Id = q.Item.Id,
                               Nome = q.Item.Nome,
                                UnidadeDeMedida = q.Item.UnidadeDeMedida
                        },
                         Usuario = new UsuarioDTO()
                         {
                             Id = q.Usuario.Id,
                             Nome = q.Usuario.Nome,
                             Email = q.Usuario.Email
                         }
                   }).ToList();

                    break;
                case "FAVORECIDO":
                    IQueryable<DespesaFavorecido> queryF = _context.DespesaFavorecido.Include(q => q.Item).Include(q=>q.Favorecido)
                .OrderByDescending(q => q.DataDaCompra);
                    if (!String.IsNullOrEmpty(filtro))
                        queryF = queryF.Where(q => q.Descricao.Contains(filtro) || q.Item.Nome.Contains(filtro) || q.Favorecido.Nome.Contains(filtro));
                    this.Response.Headers.Add("totalItems", queryF.Count().ToString());

                    despesas = queryF
                   .Skip(skip.Value)
                   .Take(take.Value).Select(q => new DespesaDTO
                   {
                       Id = q.Id,
                       Descricao = q.Descricao,
                       DataDaCompra = q.DataDaCompra,
                       Quantidade = q.Quantidade,
                       Tipo = q.Tipo,
                       ValorUnitario = q.ValorUnitario,
                       Item = new ItemDespesaDTO()
                       {
                           Id = q.Item.Id,
                           Nome = q.Item.Nome,
                           UnidadeDeMedida = q.Item.UnidadeDeMedida
                       },
                       Favorecido = new DespesaFavorecidoDTO()
                       {
                            Id = q.Favorecido.CodFavorecido,
                             Apelido = q.Favorecido.Apelido,
                              Cpf = q.Favorecido.Cpf,
                               Nome = q.Favorecido.Nome
                       },
                       Usuario = new UsuarioDTO()
                       {
                           Id = q.Usuario.Id,
                           Nome = q.Usuario.Nome,
                           Email = q.Usuario.Email
                       }
                   }).ToList();
                    break;
                case "SOPA":
                    IQueryable<DespesaSopa> queryS = _context.DespesaSopa.Include(q => q.Item)
               .OrderByDescending(q => q.DataDaCompra);
                    if (!String.IsNullOrEmpty(filtro))
                        queryS = queryS.Where(q => q.Descricao.Contains(filtro) || q.Item.Nome.Contains(filtro));
                    this.Response.Headers.Add("totalItems", queryS.Count().ToString());

                    despesas = queryS
                   .Skip(skip.Value)
                   .Take(take.Value).Select(q => new DespesaDTO
                   {
                       Id = q.Id,
                       Descricao = q.Descricao,
                       DataDaCompra = q.DataDaCompra,
                       Quantidade = q.Quantidade,
                       Tipo = q.Tipo,
                       ValorUnitario = q.ValorUnitario,
                       Item = new ItemDespesaDTO()
                       {
                           Id = q.Item.Id,
                           Nome = q.Item.Nome,
                           UnidadeDeMedida = q.Item.UnidadeDeMedida
                       },
                       Usuario = new UsuarioDTO()
                       {
                           Id = q.Usuario.Id,
                           Nome = q.Usuario.Nome,
                           Email = q.Usuario.Email
                       }
                   }).ToList();

                    break;
            }






            return despesas;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Despesa despesa =_context.Despesa
                .Include(q=>q.Item)
                .Include(q=>q.Usuario)                
                .SingleOrDefault(q => q.Id == id);

            if (despesa == null)
                return new NotFoundResult();

            DespesaDTO dto = new DespesaDTO();
            if(despesa.GetType() == typeof(DespesaAssociacao))
            {
                dto.Id = despesa.Id;
                dto.Descricao = despesa.Descricao;
                dto.DataDaCompra = despesa.DataDaCompra;
                dto.ValorUnitario = despesa.ValorUnitario;
                dto.Quantidade = despesa.Quantidade;
                dto.Tipo = despesa.Tipo;
                dto.Item = new ItemDespesaDTO()
                {
                    Id = despesa.Item.Id,
                    Nome = despesa.Item.Nome,
                    UnidadeDeMedida = despesa.Item.UnidadeDeMedida
                };
                dto.Usuario = new UsuarioDTO()
                {
                    Id = despesa.Usuario.Id,
                    Nome = despesa.Usuario.Nome,
                    Email = despesa.Usuario.Email
                };
            }
            else if (despesa.GetType() == typeof(DespesaSopa))
            {
                dto.Id = despesa.Id;
                dto.Descricao = despesa.Descricao;
                dto.DataDaCompra = despesa.DataDaCompra;
                dto.ValorUnitario = despesa.ValorUnitario;
                dto.Quantidade = despesa.Quantidade;
                dto.Tipo = despesa.Tipo;
                dto.Item = new ItemDespesaDTO()
                {
                    Id = despesa.Item.Id,
                    Nome = despesa.Item.Nome,
                    UnidadeDeMedida = despesa.Item.UnidadeDeMedida
                };
                dto.Usuario = new UsuarioDTO()
                {
                    Id = despesa.Usuario.Id,
                    Nome = despesa.Usuario.Nome,
                    Email = despesa.Usuario.Email
                };
            }
            else if (despesa.GetType() == typeof(DespesaFavorecido))
            {
                dto.Id = despesa.Id;
                dto.Descricao = despesa.Descricao;
                dto.DataDaCompra = despesa.DataDaCompra;
                dto.ValorUnitario = despesa.ValorUnitario;
                dto.Quantidade = despesa.Quantidade;
                dto.Tipo = despesa.Tipo;
                dto.Item = new ItemDespesaDTO()
                {
                    Id = despesa.Item.Id,
                    Nome = despesa.Item.Nome,
                    UnidadeDeMedida = despesa.Item.UnidadeDeMedida
                };
                dto.Usuario = new UsuarioDTO()
                {
                    Id = despesa.Usuario.Id,
                    Nome = despesa.Usuario.Nome,
                    Email = despesa.Usuario.Email
                };

                _context.Favorecido.Where(q => q.CodFavorecido == ((DespesaFavorecido)despesa).CodFavorecido).Load();
                dto.Favorecido = new DespesaFavorecidoDTO()
                {

                    Id = ((DespesaFavorecido)despesa).Favorecido.CodFavorecido,
                    Apelido = ((DespesaFavorecido)despesa).Favorecido.Apelido,
                    Cpf = ((DespesaFavorecido)despesa).Favorecido.Cpf,
                    Nome = ((DespesaFavorecido)despesa).Favorecido.Nome

                };
            }






            this.Response.Headers.Add("totalItems", "1");
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DespesaDTO dto)
        {
            if (ModelState.IsValid && (dto.Tipo.ToUpper() == "ASSOCIACAO" || dto.Tipo.ToUpper() == "FAVORECIDO" || dto.Tipo.ToUpper() == "SOPA"))
            {
                Usuario user = await _userManager.GetUserAsync(HttpContext.User);
                switch (dto.Tipo)
                {
                    case "ASSOCIACAO":
                        try
                        {
                            ItemAssociacao ia = _context.ItemAssociacao.Single(q => q.Id == dto.Item.Id);
                            DespesaAssociacao da = new DespesaAssociacao
                            {

                                
                                DataDaCompra = dto.DataDaCompra,
                                Descricao = dto.Descricao,
                                Quantidade = dto.Quantidade,
                                ValorUnitario = dto.ValorUnitario,
                                Item = ia,
                                Usuario=user
                            };
                            _context.DespesaAssociacao.Add(da);
                            _context.SaveChanges();
                            dto.Id = da.Id;
                        }
                        catch (Exception e)
                        {
                            //ModelState.AddModelError("Item", "Este item já está cadastrado");
                            return new BadRequestObjectResult(ModelState);
                        }
                        break;
                    case "FAVORECIDO":
                        try
                        {
                            ItemFavorecido ifavorecido = _context.ItemFavorecido.Single(q => q.Id == dto.Item.Id);
                            Favorecido favorecido = _context.Favorecido.Single(q => q.CodFavorecido == dto.Favorecido.Id);
                            DespesaFavorecido df = new DespesaFavorecido
                            {

                                
                                DataDaCompra = dto.DataDaCompra,
                                Descricao = dto.Descricao,
                                Quantidade = dto.Quantidade,
                                ValorUnitario = dto.ValorUnitario,
                                Item = ifavorecido,
                                Favorecido = favorecido,
                                 Usuario= user

                            };
                            _context.DespesaFavorecido.Add(df);
                            _context.SaveChanges();
                            dto.Id = df.Id;
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("Favorecido", "Este favorecido não existe mais");
                            return new BadRequestObjectResult(ModelState);
                        }

                        break;
                    case "SOPA":
                        try
                        {
                            ItemSopa isopa = _context.ItemSopa.Single(q => q.Id == dto.Item.Id);
                            DespesaSopa ds = new DespesaSopa
                            {

                                
                                DataDaCompra = dto.DataDaCompra,
                                Descricao = dto.Descricao,
                                Quantidade = dto.Quantidade,
                                ValorUnitario = dto.ValorUnitario,
                                Item = isopa,
                                 Usuario= user
                            };
                            _context.DespesaSopa.Add(ds);
                            _context.SaveChanges();
                            dto.Id = ds.Id;
                        }
                        catch (Exception e)
                        {
                            //ModelState.AddModelError("Item", "Este item já está cadastrado");
                            return new BadRequestObjectResult(ModelState);
                        }
                        break;
                }
                return new ObjectResult(dto);


            }
            else
            {
                if (ModelState.IsValid)
                    ModelState.AddModelError("Tipo", "Valores aceitados = ['ASSOCIACAO','FAVORECIDO','SOPA']");
                return new BadRequestObjectResult(ModelState);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]DespesaDTO dto)
        {
            if (id != dto.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            if (ModelState.IsValid)
            {
                Usuario user = await _userManager.GetUserAsync(HttpContext.User);
                Despesa despesa = _context.Despesa.SingleOrDefault(q => q.Id == id);
                if (despesa == null)
                    return new BadRequestResult();
                Item item = _context.Item.SingleOrDefault(q => q.Id == dto.Item.Id);
                if (item == null)
                {
                    ModelState.AddModelError("Item", "Item inválido");
                    return new BadRequestObjectResult(ModelState);
                }
                despesa.Item = item;
                despesa.Quantidade = dto.Quantidade;
                despesa.ValorUnitario = dto.ValorUnitario;
                despesa.Usuario = user;


                if (despesa.GetType() == typeof(DespesaAssociacao))
                {
                    DespesaAssociacao desp = (DespesaAssociacao)despesa;
                    //nenhum outro campo para atualizar
                    try
                    {
                        _context.SaveChanges();
                        return new ObjectResult(dto);
                    }
                    catch (Exception e)
                    {
                        return new BadRequestObjectResult(ModelState);
                    }
                }
                else if (despesa.GetType() == typeof(DespesaFavorecido))
                {
                    DespesaFavorecido desp = (DespesaFavorecido)despesa;

                    try
                    {
                        Favorecido f = _context.Favorecido.Single(q => q.CodFavorecido == dto.Favorecido.Id);
                        _context.SaveChanges();
                        return new ObjectResult(dto);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Favorecido", "Favorecido incorreto");
                        return new BadRequestObjectResult(ModelState);
                    }
                }
                else if (despesa.GetType() == typeof(DespesaSopa))
                {
                    DespesaSopa desp = (DespesaSopa)despesa;
                    //nenhum outro campo para atualizar
                    try
                    {
                        _context.SaveChanges();
                        return new ObjectResult(dto);
                    }
                    catch (Exception e)
                    {
                        return new BadRequestObjectResult(ModelState);
                    }
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
            Despesa doador = _context.Despesa.Single(q => q.Id == id);            
            _context.Despesa.Remove(doador);

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

        
        [HttpGet("pdf/{id}")]
        public IActionResult GetFile(string id)
        {
            var fileName = $"export.pfd";
            var filepath = $"Downloads/{fileName}";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/pdf", fileName);
        }
    }
}
