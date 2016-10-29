using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Drawing;
using Syncfusion.XlsIO;
using CustomExtensions;
using System.IO;

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
                .OrderBy(q => q.Nome);

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
                ModelState.AddModelError("ResultadoSopa", "Não é possível deletar. Este modelo já foi aplicado a pelo menos um Resultado de Sopa");
                return new BadRequestObjectResult(ModelState);
            }
        }

        [HttpGet("excel")]
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro)
        {

            IQueryable<ModeloDeReceita> query = _context.ModeloDeReceita.Include(q => q.Itens).ThenInclude(q => q.Item)
                .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Descricao.Contains(filtro) || q.Nome.Contains(filtro));

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
            sheet.Name = "Modelos";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 30;
            sheet.Range[1, 3].ColumnWidth = 30;
            sheet.Range[1, 4].ColumnWidth = 15;
            sheet.Range[1, 5].ColumnWidth = 15;
            sheet.Range[1, 6].ColumnWidth = 15;


            sheet.Range[1, 1, 1, 6].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Modelos de Receita";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Nome";
            sheet.Range[3, 3].Text = "Descrição";
            sheet.Range[3, 4].Text = "Item";
            sheet.Range[3, 5].Text = "Quantidade";
            sheet.Range[3, 6].Text = "Unidade de medida";


            IStyle style = sheet[3, 1, 3, 6].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach (var m in query)
            {
                if (m.Itens.Count > 0)
                {
                    foreach (var i in m.Itens)
                    {
                        sheet.Range[linha, 1].Number = m.Id;
                        sheet.Range[linha, 2].Text = m.Nome;
                        sheet.Range[linha, 3].Text = m.Descricao;
                        sheet.Range[linha, 4].Text = i.Item.Nome;                        
                        sheet.Range[linha, 5].NumberFormat = "#,##0.00_ ;[red]-#,##0.00 ";
                        sheet.Range[linha, 5].Number = i.Quantidade;
                        sheet.Range[linha, 6].Text = i.Item.UnidadeDeMedida;
                        linha++;
                    }
                }
                else
                {
                    sheet.Range[linha, 1].Number = m.Id;
                    sheet.Range[linha, 2].Text = m.Nome;
                    sheet.Range[linha, 3].Text = m.Descricao;
                    linha++;
                }

                
            }

        

            #endregion

                    string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "ModeloDeReceita.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "ModeloDeReceita.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
    }
}
