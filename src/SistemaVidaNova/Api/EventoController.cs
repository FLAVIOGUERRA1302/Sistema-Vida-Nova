using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class EventoController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        public EventoController(VidaNovaContext context,UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<EventoDTO> Get([FromQuery]DateTime? start, [FromQuery]DateTime? end, [FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro, [FromQuery]bool? comAgendaMotorista)
        {

            IQueryable<Evento> query = _context.Evento
                .Include(q => q.Usuario)
            .Include(q => q.Voluntarios)
            .Include(q => q.Interessados)
            .Include(q => q.Favorecidos)
            .Include(q => q.Doadores);

            if (start!=null && end != null)//busca pelo calendario
            {
                query = query.Where(q => q.DataInicio >= start && q.DataInicio <= end);
            }
            else { // busca pela lista
                if (skip == null)
                    skip = 0;
                if (take == null)
                    take = 1000;

                 query = query
                    .OrderByDescending(q => q.DataInicio);

                if (!String.IsNullOrEmpty(filtro))
                    query = query.Where(q => q.Titulo.Contains(filtro));

                this.Response.Headers.Add("totalItems", query.Count().ToString());
                query = query
                .Skip(skip.Value)
                .Take(take.Value);
            }
           


            List<EventoDTO> eventos = (from q in query
                                       select new EventoDTO
                                                 {
                                                     id = q.CodEvento,
                                                     title = q.Titulo,
                                                     descricao = q.Descricao,
                                                     color = q.Cor,
                                                     textColor = q.CorDaFonte,
                                                     start = q.DataInicio,
                                                     end = q.DataFim,
                                                     
                                                     valorArrecadado = q.ValorArrecadado,
                                                     relato = q.Relato,
                                                       Usuario = new UsuarioDTOR()
                                                       {
                                                            Id = q.Usuario.Id,
                                                             Email = q.Usuario.Email,
                                                              Nome = q.Usuario.Nome
                                                       }     ,
                                           Tipo="EVENTO",
                                           participantes= q.Favorecidos.Count+ q.Interessados.Count+ q.Voluntarios.Count+ q.Doadores.Count


                                       }).ToList();

            if(comAgendaMotorista!= null && comAgendaMotorista.Value)
            {
                
                var queryDoacao = _context.DoacaoObjeto
                    .Include(q=>q.Doador)
                    .Include(q=>q.Voluntario)                    
                    .Where(q => q.DataDeRetirada >= start && q.DataDeRetirada <= end);
                foreach( var doacao in queryDoacao)
                {
                    string nomeRazaoSocial = doacao.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)doacao.Doador).Nome : ((PessoaJuridica)doacao.Doador).RazaoSocial;
                    EventoDTO dto = new EventoDTO
                    {
                        id = doacao.Id,
                        title = "Retirada de Doação.",
                        descricao = "",
                        color = "#FF0000",
                        textColor ="#FFFFFF",
                        start = doacao.DataDeRetirada,
                        end = doacao.DataDeRetirada,

                        valorArrecadado = 0,
                        relato = "",                        
                        Tipo = "DOACAO"


                    };
                    eventos.Add(dto);
                }


                    
            }

              return eventos;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Evento eve = _context.Evento
                    .Include(q => q.Interessados)
                    .ThenInclude(q=>q.Interessado)                    
                    .Include(q => q.Voluntarios)
                    .ThenInclude(q => q.Voluntario)
                    .Include(q=>q.Usuario)
                    .Include(q => q.Doadores)
                    .ThenInclude(q => q.Doador)
                    .Include(q => q.Favorecidos)
                    .ThenInclude(q => q.Favorecido)
                    .SingleOrDefault(q => q.CodEvento == id);
            if (eve == null)
                return new NotFoundResult();
            /*if (eve.Interessados.Count > 0)
                _context.InteressadoEvento.Where(q => q.CodEvento == id).Include(q => q.Interessado).Load();
            if (eve.Voluntarios.Count > 0)
                _context.VoluntarioEvento.Where(q => q.CodEvento == id).Include(q => q.Voluntario).Load();*/

            EventoDTO dto = new EventoDTO
            {
                id = eve.CodEvento,
                title = eve.Titulo,
                descricao = eve.Descricao,
                color = eve.Cor,
                textColor = eve.CorDaFonte,
                start = eve.DataInicio,
                end = eve.DataFim,
                
                valorArrecadado = eve.ValorArrecadado,
                relato = eve.Relato,
                 
                Usuario = new UsuarioDTOR()
                {
                    Id = eve.Usuario.Id,
                    Email = eve.Usuario.Email,
                    Nome = eve.Usuario.Nome
                },

                voluntarios = eve.Voluntarios.Select(q=>new VoluntarioDTOR{
                 Id = q.IdVoluntario,
                 Nome = q.Voluntario.Nome,
                 Cpf = q.Voluntario.Cpf}).ToList(),
                interessados = eve.Interessados.Select(q => new InteressadoDTOR
                {
                    Id = q.Interessado.CodInteressado,
                    Nome = q.Interessado.Nome
                }).ToList(),

                favorecidos = eve.Favorecidos.Select(q => new FavorecidoDTOR
                {
                    Id = q.Favorecido.CodFavorecido,
                     Nome = q.Favorecido.Nome
                }).ToList()

            };

            dto.doadoresPj = new List<DoadorDTOR>();
            dto.doadoresPf = new List<DoadorDTOR>();
            foreach (var doadorEvento in eve.Doadores)
            {
                if(doadorEvento.Doador.GetType() == typeof(PessoaFisica))
                {
                    dto.doadoresPf.Add(new DoadorDTOR()
                    {
                        Id = doadorEvento.Doador.CodDoador,
                        NomeRazaoSocial = ((PessoaFisica)doadorEvento.Doador).Nome
                    });

                } else if (doadorEvento.Doador.GetType() == typeof(PessoaJuridica))
                {
                    dto.doadoresPj.Add(new DoadorDTOR()
                    {
                        Id = doadorEvento.Doador.CodDoador,
                        NomeRazaoSocial = ((PessoaJuridica)doadorEvento.Doador).RazaoSocial
                    });
                }
            }

            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EventoDTO evento)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);
                Evento novo = new Evento()
                {

                    Titulo = evento.title,
                Descricao = evento.descricao,
                Cor = evento.color,
                CorDaFonte = evento.textColor,
                DataInicio = evento.start,
                DataFim = evento.end,
                
                ValorArrecadado = evento.valorArrecadado,
                Relato = evento.relato,
                 Usuario=usuario
                 
                 
            };
                _context.Evento.Add(novo);
                try
                {
                    _context.SaveChanges();
                    
                    return new ObjectResult(evento);
                }
                catch {

                }
                


            }
            
                return new BadRequestObjectResult(ModelState);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EventoDTO evento)
        {
            if (id != evento.id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {

                

                Evento e = _context.Evento
                    .Include(q=>q.Interessados)
                    .Include(q=>q.Voluntarios)
                    .Include(q => q.Favorecidos)
                    .Include(q => q.Doadores)
                    .Single(q => q.CodEvento == id);

                e.Titulo = evento.title;
                e.Descricao = evento.descricao;
                e.Cor = evento.color;
                e.CorDaFonte = evento.textColor;
                e.DataInicio = evento.start;
                e.DataFim = evento.end;
                
                e.ValorArrecadado = evento.valorArrecadado;
                e.Relato = evento.relato;



                //interessados
                if (evento.interessados != null)
                {
                    //verifica quem está presente e quem saiu
                    List<InteressadoEvento> corretos = new List<InteressadoEvento>();
                    foreach (var inter in evento.interessados)
                    {
                        var eventoInteressado = e.Interessados.SingleOrDefault(q => q.CodInteressado == inter.Id);
                        if (eventoInteressado == null)
                        {
                            eventoInteressado = new InteressadoEvento { CodEvento = e.CodEvento, CodInteressado = inter.Id };
                            corretos.Add(eventoInteressado);
                            e.Interessados.Add(eventoInteressado);


                        }
                        corretos.Add(eventoInteressado);

                    }
                    //remove incorretos
                    var incorretos = e.Interessados.Except(corretos);
                    foreach (var incorreto in incorretos.ToArray())
                        e.Interessados.Remove(incorreto);

                }
                else
                {
                    e.Interessados.Clear();
                }


                if (evento.voluntarios != null)
                {
                    //verifica quem está presente e quem saiu
                    List<VoluntarioEvento> corretos = new List<VoluntarioEvento>();
                    foreach (var volunt in evento.voluntarios)
                    {
                        var eventoVoluntario = e.Voluntarios.SingleOrDefault(q => q.IdVoluntario == volunt.Id);
                        if (eventoVoluntario == null)
                        {
                            eventoVoluntario = new VoluntarioEvento { CodEvento = e.CodEvento, IdVoluntario = volunt.Id };
                            corretos.Add(eventoVoluntario);
                            e.Voluntarios.Add(eventoVoluntario);

                        }
                        corretos.Add(eventoVoluntario);

                    }
                    //remove incorretos
                    var incorretos = e.Voluntarios.Except(corretos);
                    foreach (var incorreto in incorretos.ToArray())
                        e.Voluntarios.Remove(incorreto);
                }
                else
                {
                    e.Voluntarios.Clear();
                }

                //favorecidos
                if (evento.favorecidos != null)
                {
                    //verifica quem está presente e quem saiu
                    List<FavorecidoEvento> corretos = new List<FavorecidoEvento>();
                    foreach (var favorecido in evento.favorecidos)
                    {
                        var eventoFavorecido = e.Favorecidos.SingleOrDefault(q => q.CodFavorecido == favorecido.Id);
                        if (eventoFavorecido == null)
                        {
                            eventoFavorecido = new FavorecidoEvento { CodEvento = e.CodEvento,  CodFavorecido = favorecido.Id };
                            corretos.Add(eventoFavorecido);
                            e.Favorecidos.Add(eventoFavorecido);

                        }
                        corretos.Add(eventoFavorecido);

                    }
                    //remove incorretos
                    var incorretos = e.Favorecidos.Except(corretos);
                    foreach (var incorreto in incorretos.ToArray())
                        e.Favorecidos.Remove(incorreto);
                }
                else
                {
                    e.Favorecidos.Clear();
                }

                //doadores
                if (evento.doadoresPf == null)
                    evento.doadoresPf = new List<DoadorDTOR>();
                if (evento.doadoresPj == null)
                    evento.doadoresPj = new List<DoadorDTOR>();
                List<DoadorDTOR> doadores = evento.doadoresPf.Concat(evento.doadoresPj).ToList();

                if (doadores.Count>0)
                {
                    

                    //verifica quem está presente e quem saiu
                    List<DoadorEvento> corretos = new List<DoadorEvento>();
                    foreach (var doador in doadores)
                    {
                        var eventoDoador = e.Doadores.SingleOrDefault(q => q.CodDoador == doador.Id);
                        if (eventoDoador == null)
                        {
                            eventoDoador = new DoadorEvento { CodEvento = e.CodEvento,  CodDoador = doador.Id };
                            corretos.Add(eventoDoador);
                            e.Doadores.Add(eventoDoador);

                        }
                        corretos.Add(eventoDoador);

                    }
                    //remove incorretos
                    var incorretos = e.Doadores.Except(corretos);
                    foreach (var incorreto in incorretos.ToArray())
                        e.Doadores.Remove(incorreto);
                }
                else
                {
                    e.Doadores.Clear();
                }






                _context.SaveChanges();

                return new ObjectResult(evento);
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
            Evento evento = _context.Evento.Single(q => q.CodEvento == id);
            _context.Evento.Remove(evento);
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

            IQueryable<Evento> query = _context.Evento
                .Include(q => q.Usuario)
                .Include(q=>q.Interessados)
                .ThenInclude(q=>q.Interessado)
                .Include(q=>q.Voluntarios)
                .ThenInclude(q => q.Voluntario)
                .Include(q => q.Favorecidos)
                .ThenInclude(q => q.Favorecido)
                .Include(q => q.Doadores)
                .ThenInclude(q => q.Doador)
               .OrderByDescending(q => q.DataInicio);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Titulo.Contains(filtro));

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
            sheet.Name = "Eventos";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 30;
            sheet.Range[1, 3].ColumnWidth = 30;
            sheet.Range[1, 4].ColumnWidth = 15;
            sheet.Range[1, 5].ColumnWidth = 15;
            sheet.Range[1, 6].ColumnWidth = 15;
            sheet.Range[1, 7].ColumnWidth = 15;
            sheet.Range[1, 8].ColumnWidth = 15;
            sheet.Range[1, 9].ColumnWidth = 15;
            sheet.Range[1, 10].ColumnWidth = 40;
            sheet.Range[1, 11].ColumnWidth = 40;
            sheet.Range[1, 12].ColumnWidth = 40;
            sheet.Range[1, 13].ColumnWidth = 40;
            sheet.Range[1, 14].ColumnWidth = 40;


            sheet.Range[1, 1, 1, 14].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Eventos";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Título";
            sheet.Range[3, 3].Text = "Desrição";
            sheet.Range[3, 4].Text = "Dt. Inicio";
            sheet.Range[3, 5].Text = "Dt. Fim";
            sheet.Range[3, 6].Text = "Hora Inicio";
            sheet.Range[3, 7].Text = "Hora Fim";
            sheet.Range[3, 8].Text = "Valor arrecadado";
            sheet.Range[3, 9].Text = "Relato";
            sheet.Range[3, 10].Text = "Voluntários";
            sheet.Range[3, 11].Text = "Interessados";
            sheet.Range[3, 12].Text = "Favorecidos";
            sheet.Range[3, 13].Text = "Doadores PJ";
            sheet.Range[3, 14].Text = "Doadores PF";


            IStyle style = sheet[3, 1, 3, 14].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach (var v in query)
            {
                sheet.Range[linha, 1].Number = v.CodEvento;
                sheet.Range[linha, 2].Text = v.Titulo;
                sheet.Range[linha, 3].Text = v.Descricao;
                sheet.Range[linha, 4].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 4].DateTime = v.DataInicio;
                sheet.Range[linha, 5].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 5].DateTime = v.DataFim;
                sheet.Range[linha, 6].NumberFormat = "h:mm;@";
                sheet.Range[linha, 6].DateTime = v.DataInicio;
                sheet.Range[linha, 7].NumberFormat = "h:mm;@";
                sheet.Range[linha, 7].DateTime = v.DataFim;
                sheet.Range[linha, 8].NumberFormat = "R$ #,##0.00_ ;[red]R$ -#,##0.00 ";
                sheet.Range[linha, 8].Number = v.ValorArrecadado;
                sheet.Range[linha, 9].Text = v.Relato;
                sheet.Range[linha, 10].Text = string.Join(";", v.Voluntarios.Select(c => c.Voluntario.Nome).ToList()); ;
                sheet.Range[linha, 11].Text = string.Join(";", v.Interessados.Select(c => c.Interessado.Nome).ToList()); ;
                sheet.Range[linha, 12].Text = string.Join(";", v.Favorecidos.Select(c => c.Favorecido.Nome).ToList()); ;
                string pjs = "";
                string pjf = "";
                foreach(var ed in v.Doadores)
                {
                    if(ed.Doador.GetType() == typeof(PessoaFisica))
                    {
                        pjf += ((PessoaFisica)ed.Doador).Nome+";";
                    }
                    else if (ed.Doador.GetType() == typeof(PessoaJuridica))
                    {
                        pjs += ((PessoaJuridica)ed.Doador).RazaoSocial + ";";
                    }
                }

                sheet.Range[linha, 13].Text = pjs;
                sheet.Range[linha, 14].Text = pjf;


                linha++;
            }

            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Evento.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Evento.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
    }
}
