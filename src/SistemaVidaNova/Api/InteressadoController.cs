using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Syncfusion.XlsIO;
using CustomExtensions;
using Syncfusion.Drawing;
using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class InteressadoController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public InteressadoController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<InteressadoDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<Interessado> query = _context.Interessado                
                .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<InteressadoDTO> interessados = query
                .Skip(skip.Value)
                .Take(take.Value).Select(v => new InteressadoDTO
                {
                    Id = v.CodInteressado,
                    Email = v.Email,
                    Nome = v.Nome,
                    Celular = v.Celular,
                    Telefone = v.Telefone
                }).ToList();

              return interessados;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Interessado q = _context.Interessado.SingleOrDefault(i => i.CodInteressado == id);
            if (q == null)
                return new NotFoundResult();

            InteressadoDTO dto = new InteressadoDTO
            {
                Id = q.CodInteressado,
                Celular = q.Celular,
                Email = q.Email,
                Nome = q.Nome,
                Telefone = q.Telefone/*,
                Eventos = q.Eventos.OrderByDescending(e => e.DataInicio)
                    .Select(e => new EventoDTO
                    {
                        id = e.CodEvento,
                        descricao = e.Descricao,
                        title = e.Titulo,
                        start = e.DataInicio,
                        end = e.DataFim,
                        valorDeEntrada = e.ValorDeEntrada,
                        valorArrecadado = e.ValorArrecadado
                    }).ToList()*/
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]InteressadoDTO i)
        {
            if (ModelState.IsValid)
            {
                Interessado novo = new Interessado()
                {
                    Nome = i.Nome,
                    Celular = i.Celular,
                    Email = i.Email,
                    Telefone = i.Telefone
                };
                _context.Interessado.Add(novo);
                try
                {
                    _context.SaveChanges();
                    i.Id = novo.CodInteressado;
                    return new ObjectResult(i);
                }
                catch {
                    ModelState.AddModelError("Email", "Este email já está cadastrado");
                    return new BadRequestObjectResult(ModelState);
                }
                


            }
            
                return new BadRequestObjectResult(ModelState);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]InteressadoDTO interessado)
        {
            if (id != interessado.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                Interessado i = _context.Interessado.Single(q => q.CodInteressado == id);


                i.Nome = interessado.Nome;
                i.Celular = interessado.Celular;
                i.Telefone = interessado.Telefone;
                i.Email = interessado.Email;
                try
                {
                    _context.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("Email", "Este email já está cadastrado");
                    return new BadRequestObjectResult(ModelState);
                }

                return new ObjectResult(interessado);
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
            Interessado interessado = _context.Interessado.Single(q => q.CodInteressado == id);
            _context.Interessado.Remove(interessado);
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

        [HttpGet("excel")]
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro)
        {

            IQueryable<Interessado> query = _context.Interessado               
               .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

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
            sheet.Name = "Interessados";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 30;
            sheet.Range[1, 3].ColumnWidth = 30;
            sheet.Range[1, 4].ColumnWidth = 15;
            sheet.Range[1, 5].ColumnWidth = 15;
           

            sheet.Range[1, 1, 1, 5].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Interessados";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Email";
            sheet.Range[3, 3].Text = "Nome";            
            sheet.Range[3, 4].Text = "Celular";
            sheet.Range[3, 5].Text = "Telefone";
            

            IStyle style = sheet[3, 1, 3, 5].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach (var v in query)
            {
                sheet.Range[linha, 1].Number = v.CodInteressado;
                sheet.Range[linha, 2].Text = v.Email;
                sheet.Range[linha, 3].Text = v.Nome;                
                sheet.Range[linha, 4].Text = v.Celular.ToTelefone();
                sheet.Range[linha, 5].Text = v.Telefone.ToTelefone();
                
                linha++;
            }

            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Interessado.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Interessado.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
    }
}
