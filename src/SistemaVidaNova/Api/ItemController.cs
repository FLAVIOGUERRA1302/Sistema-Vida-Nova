using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using System.IO;

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

            IQueryable<Item> query = from q in _context.Item
                                     orderby q.Destino, q.Nome
                                     select q;
                

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro) );

            if (!String.IsNullOrEmpty(destino))
            {
                destino = destino.ToUpper();                
                query = query.Where(q => q.Destino == destino);

            }



            this.Response.Headers.Add("totalItems", query.Count().ToString());

            var itens = query
               .Skip(skip.Value)
               .Take(take.Value).ToList();

            List<ItemDTO> itensDto = itens.Select(v => new ItemDTO
            {
                Id = v.Id,
                Nome = v.Nome,
                Destino = v.Destino,
                UnidadeDeMedida = v.UnidadeDeMedida



            }).ToList();
              return itensDto;
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

        [HttpGet("excel")]
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro, [FromQuery]string destino)
        {

            IQueryable<Item> query = _context.Item
               .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            if (!String.IsNullOrEmpty(destino))
            {
                destino = destino.ToUpper();
                query = query.Where(q => q.Destino == destino);

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
            sheet.Name = "Itens";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 30;
            sheet.Range[1, 3].ColumnWidth = 30;
            sheet.Range[1, 4].ColumnWidth = 30;
            


            sheet.Range[1, 1, 1, 4].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Itens";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Nome";
            sheet.Range[3, 3].Text = "Destino";
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
                sheet.Range[linha, 3].Text = q.Destino;
                sheet.Range[linha, 4].Text = q.UnidadeDeMedida;
                

                linha++;
            }

            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Item.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Item.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }

    }
}
