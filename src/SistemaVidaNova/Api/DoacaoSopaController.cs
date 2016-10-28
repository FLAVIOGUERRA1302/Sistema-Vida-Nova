using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SistemaVidaNova.Services;
using Microsoft.AspNetCore.Identity;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class DoacaoSopaController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        private readonly IEstoqueManager _estoqueManager;
        private readonly UserManager<Usuario> _userManager;
        public DoacaoSopaController(VidaNovaContext context, IEstoqueManager estoqueManager, UserManager<Usuario> userManager)
        {
            _context = context;
            _estoqueManager = estoqueManager;
            _userManager = userManager;
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
            {
                var doadores = _context.DoadorPessoaFisica.Where(q => q.Nome.Contains(filtro)).Select(q => (Doador)q)
                    .Concat(_context.DoadorPessoaJuridica.Where(q => q.RazaoSocial.Contains(filtro)).Select(q => (Doador)q));

                query = query.Where(q => q.Descricao.Contains(filtro) || doadores.Contains(q.Doador) || q.Item.Nome.Contains(filtro));

            }

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
        public async Task<IActionResult> Post([FromBody]DoacaoSopaDTO dto)
        {
            if (ModelState.IsValid)
            {

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
                    //atualiza o estoque
                    Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);                    
                    _estoqueManager.DarEntrada(usuario, item, novo.Quantidade);

                    dto.Id = novo.Id;
                    return new ObjectResult(dto);
                }
                catch
                {

                    return new BadRequestObjectResult(ModelState);
                }



            }

            return new BadRequestObjectResult(ModelState);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]DoacaoSopaDTO dto)
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
                double diferencaQuantidade = dd.Quantidade - dto.Quantidade;
                dd.Doador = doador;
                dd.Data = dto.DataDaDoacao;
                dd.Descricao = dto.Descricao;
                dd.Item = item;
                dd.Quantidade = dto.Quantidade;


                try
                {
                    _context.SaveChanges();
                    Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);

                    //atualiza o estoque
                    if (diferencaQuantidade>0)
                        _estoqueManager.DarSaida(usuario, item, diferencaQuantidade);
                    else if (diferencaQuantidade < 0)
                        _estoqueManager.DarEntrada(usuario, item, -diferencaQuantidade);


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
        public async Task<IActionResult> Delete(int id)
        {
            DoacaoSopa ds = _context.DoacaoSopa.Single(q => q.Id == id);
            double quantidade = ds.Quantidade;
            Item item = ds.Item;
            _context.DoacaoSopa.Remove(ds);
            try
            {
                _context.SaveChanges();
                //atualiza o estoque
                Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);                               
                _estoqueManager.DarSaida(usuario, item, quantidade);
                
                return new NoContentResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        [HttpGet("excel")]
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro)
        {

            IQueryable<DoacaoSopa> query = _context.DoacaoSopa.Include(d => d.Doador).Include(d => d.Item)
               .OrderByDescending(q => q.Data);

            if (!String.IsNullOrEmpty(filtro))
            {
                var doadores = _context.DoadorPessoaFisica.Where(q => q.Nome.Contains(filtro)).Select(q => (Doador)q)
                    .Concat(_context.DoadorPessoaJuridica.Where(q => q.RazaoSocial.Contains(filtro)).Select(q => (Doador)q));

                query = query.Where(q => q.Descricao.Contains(filtro) || doadores.Contains(q.Doador) || q.Item.Nome.Contains(filtro));

            }

            if (SaveOption == null)
                SaveOption = "ExcelXlsx";




            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.

            //Step 1 : Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();
            //Step 2 : Instantiate the excel application object.
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.Name = "Doações";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 15;
            sheet.Range[1, 3].ColumnWidth = 30;
            sheet.Range[1, 4].ColumnWidth = 15;
            sheet.Range[1, 5].ColumnWidth = 15;
            sheet.Range[1, 6].ColumnWidth = 15;
            sheet.Range[1, 7].ColumnWidth = 15;
            sheet.Range[1, 8].ColumnWidth = 15;


            sheet.Range[1, 1, 1, 8].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Doações Sopa";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Data da doação";
            sheet.Range[3, 3].Text = "Doador";
            sheet.Range[3, 4].Text = "Tipo do doador";
            sheet.Range[3, 5].Text = "Descrição";
            sheet.Range[3, 6].Text = "Item";
            sheet.Range[3, 7].Text = "Quantidade";
            sheet.Range[3, 8].Text = "Unidade de medida";


            IStyle style = sheet[3, 1, 3, 8].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach (var q in query)
            {
                sheet.Range[linha, 1].Number = q.Id;
                sheet.Range[linha, 2].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 2].DateTime = q.Data;
                sheet.Range[linha, 3].Text = q.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)q.Doador).Nome : ((PessoaJuridica)q.Doador).RazaoSocial;
                sheet.Range[linha, 4].Text = q.Doador.GetType() == typeof(PessoaFisica) ? "PF" : "PJ";
                sheet.Range[linha, 5].Text = q.Descricao;
                sheet.Range[linha, 6].Text = q.Item.Nome;
                sheet.Range[linha, 7].NumberFormat = "#,##0.00_ ;[red]-#,##0.00 ";
                sheet.Range[linha, 7].Number = q.Quantidade;
                sheet.Range[linha, 8].Text = q.Item.UnidadeDeMedida;

                linha++;
            }

            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Doação.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Doação.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
    }
}
