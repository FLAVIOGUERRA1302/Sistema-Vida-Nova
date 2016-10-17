using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SistemaVidaNova.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class EstoqueController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly IEstoqueManager _estoqueManager;
        public EstoqueController(VidaNovaContext context, UserManager<Usuario> userManager, IEstoqueManager estoqueManager)
        {
            _context = context;
            _userManager = userManager;
            _estoqueManager = estoqueManager;
        }


        [HttpGet]
        public IEnumerable<EstoqueDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro, [FromQuery]bool? somenteNegativos)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;
            if (somenteNegativos == null)
                somenteNegativos = false;
            IQueryable<Item> query = _context.Item
                .Where(q => q.Destino == "SOPA")
                .OrderBy(q => q.Nome);

            

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));
            if (somenteNegativos.Value)
                query = query.Where(q => q.QuantidadeEmEstoque < 0);

            


            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<EstoqueDTO> estoque = query
                .Skip(skip.Value)
                .Take(take.Value).Select(v => new EstoqueDTO
                {

                    Id = v.Id,
                    Nome = v.Nome,
                    UnidadeDeMedida = v.UnidadeDeMedida,
                    Quantidade = v.QuantidadeEmEstoque


                }).ToList();

            return estoque;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Item q = _context.Item.SingleOrDefault(i => i.Id == id);
            if (q == null)
                return new NotFoundResult();

            EstoqueDTO dto = new EstoqueDTO
            {

                Id = q.Id,
                Nome = q.Nome,
                UnidadeDeMedida = q.UnidadeDeMedida,
                Quantidade = q.QuantidadeEmEstoque


            };
            return new ObjectResult(dto);
        }



        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]EstoqueDTO dto)
        {
            if (id != dto.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                Item item = _context.Item.Single(q => q.Id == id);



                Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);

                try
                {
                    _estoqueManager.Ajustar(usuario, item, dto.Quantidade);
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

        
    }
}
