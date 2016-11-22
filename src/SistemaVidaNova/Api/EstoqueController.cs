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
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using System.IO;

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
                query = query.Where(q => q.QuantidadeEmEstoque <= 0);

            


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

        [HttpGet("excel")]
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro, [FromQuery]bool? somenteNegativos)
        {

            if (somenteNegativos == null)
                somenteNegativos = false;

            IQueryable<Item> query = _context.Item
                .Where(q => q.Destino == "SOPA")
               .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            if (somenteNegativos.Value)
                query = query.Where(q => q.QuantidadeEmEstoque < 0);

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
            sheet.Name = "Estoque";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 30;
            sheet.Range[1, 3].ColumnWidth = 30;
            sheet.Range[1, 4].ColumnWidth = 30;



            sheet.Range[1, 1, 1, 4].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Estoque";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Nome";
            sheet.Range[3, 3].Text = "Quantidade";
            sheet.Range[3, 4].Text = "Unidade de Medida";



            IStyle style = sheet[3, 1, 3, 4].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach (var q in query)
            {
                sheet.Range[linha, 1].Number = q.Id;
                sheet.Range[linha, 2].Text = q.Nome;
                sheet.Range[linha, 3].NumberFormat = "#,##0.00_ ;[red]-#,##0.00 ";
                sheet.Range[linha, 3].Number = q.QuantidadeEmEstoque;
                sheet.Range[linha, 4].Text = q.UnidadeDeMedida;


                linha++;
            }

            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Estoque.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Estoque.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }



    }
}
