using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using CustomExtensions;
using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class DoacaoDinheiroController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public DoacaoDinheiroController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<DoacaoDinheiroDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<DoacaoDinheiro> query = _context.DoacaoDinheiro.Include(q=>q.Doador)                
                .OrderByDescending(q => q.Data);

            if (!String.IsNullOrEmpty(filtro))
            {
                var doadores = _context.DoadorPessoaFisica.Where(q => q.Nome.Contains(filtro)).Select(q => (Doador)q)
                    .Concat(_context.DoadorPessoaJuridica.Where(q => q.RazaoSocial.Contains(filtro)).Select(q => (Doador)q));
                query = from q in query                        
                        where q.Descricao.Contains(filtro) || doadores.Contains(q.Doador)
                        select q;
            }

            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<DoacaoDinheiroDTO> doacoes = new List<DoacaoDinheiroDTO>();
            query = query.Skip(skip.Value).Take(take.Value);
            foreach( var v in query)
            {
                doacoes.Add(new DoacaoDinheiroDTO
                {
                    Id = v.Id,
                    DataDaDoacao = v.Data,
                    Descricao = v.Descricao,
                    Doador = new DoadorDTOR()
                    {
                        Id = v.Doador.CodDoador,
                        NomeRazaoSocial = v.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)v.Doador).Nome : ((PessoaJuridica)v.Doador).RazaoSocial
                    },
                    Valor = v.Valor
                });
            }
           

              return doacoes;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            DoacaoDinheiro q = _context.DoacaoDinheiro.Include(d=>d.Doador).SingleOrDefault(i => i.Id == id);
            if (q == null)
                return new NotFoundResult();

            DoacaoDinheiroDTO dto = new DoacaoDinheiroDTO
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
                Valor = q.Valor
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]DoacaoDinheiroDTO dto)
        {
            if (ModelState.IsValid)
            {
                
                Doador doador = _context.Doador.SingleOrDefault(q => q.CodDoador == dto.Doador.Id);
                if(doador == null)
                {
                    ModelState.AddModelError("Doador", "Doador inválido");
                    return new BadRequestObjectResult(ModelState);
                }
                //corrige fuso horario do js
                dto.DataDaDoacao = dto.DataDaDoacao.AddHours(-dto.DataDaDoacao.Hour);
                DoacaoDinheiro novo = new DoacaoDinheiro()
                {
                    Doador = doador,
                    Data = dto.DataDaDoacao,
                    Descricao = dto.Descricao,
                    Valor = dto.Valor
                };
                try
                {
                    _context.DoacaoDinheiro.Add(novo);
                
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
        public IActionResult Put(int id, [FromBody]DoacaoDinheiroDTO dto)
        {
            if (id != dto.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                //corrige fuso horario do js
                dto.DataDaDoacao = dto.DataDaDoacao.AddHours(-dto.DataDaDoacao.Hour);
                DoacaoDinheiro dd = _context.DoacaoDinheiro.Single(q => q.Id == id);

                Doador doador = _context.Doador.SingleOrDefault(q => q.CodDoador == dto.Doador.Id);
                if (doador == null)
                {
                    ModelState.AddModelError("Doador", "Doador inválido");
                    return new BadRequestObjectResult(ModelState);
                }

                dd.Doador = doador;
                dd.Data = dto.DataDaDoacao;
                dd.Descricao = dto.Descricao;
                dd.Valor = dto.Valor;


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
            DoacaoDinheiro dd = _context.DoacaoDinheiro.Single(q => q.Id == id);
            _context.DoacaoDinheiro.Remove(dd);
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

            IQueryable<DoacaoDinheiro> query = _context.DoacaoDinheiro.Include(q => q.Doador)
               .OrderByDescending(q => q.Data);

            if (!String.IsNullOrEmpty(filtro))
            {
                var doadores = _context.DoadorPessoaFisica.Where(q => q.Nome.Contains(filtro)).Select(q => (Doador)q)
                    .Concat(_context.DoadorPessoaJuridica.Where(q => q.RazaoSocial.Contains(filtro)).Select(q => (Doador)q));
                query = from q in query
                        where q.Descricao.Contains(filtro) || doadores.Contains(q.Doador)
                        select q;
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


            sheet.Range[1, 1, 1, 6].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Doações em dinheiro";
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
            sheet.Range[3, 6].Text = "Valor";


            IStyle style = sheet[3, 1, 3, 6].CellStyle;
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
                sheet.Range[linha, 6].NumberFormat = "R$ #,##0.00_ ;[red]R$ -#,##0.00 ";
                sheet.Range[linha, 6].Number = q.Valor;

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
