using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;
using Microsoft.AspNetCore.Http;
using SistemaVidaNova.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http;
using SistemaVidaNova.Models.DTOs;
using SistemaVidaNova.Services;
using MimeKit;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Authorize]    
    [Route("api/[controller]")]
    public class InformativoController : Controller
    {
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailSender _emailSender;

        private IHostingEnvironment _environment;
        public InformativoController(
            VidaNovaContext context,
            UserManager<Usuario> userManager,
            IEmailSender emailSender,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _environment = environment;
        }

        // GET: api/values
        [HttpGet]
        public async Task<InformativoDTO> Get()
        {
            Usuario user = await _userManager.GetUserAsync(HttpContext.User);
            Informativo info = _context.Informativo
                 .Include(q => q.Attachments)
                 .Include(q=> q.Doadores)
                 .ThenInclude(q => q.Doador)
                 .Include(q => q.Usuario)
                 .Include(q => q.Usuarios)
                 .ThenInclude(q => q.Usuario)
                 .Include(q => q.Voluntarios)
                 .ThenInclude(q => q.Voluntario)
                 .Where(q => q.IdUsuario == user.Id)
                 .SingleOrDefault();
            if (info == null)
            {
                info = new Informativo()
                {
                    Subject = "",
                    Usuario = user,
                    Body = "",
                    Doadores = new List<InformativoDoador>(),
                    Usuarios = new List<InformativoUsuario>(),
                    Voluntarios = new List<InformativoVoluntario>(),
                    Attachments = new List<Attachment>()
                      

                };
                _context.Informativo.Add(info);
                _context.SaveChanges();
                
            }

            InformativoDTO infoDto = new InformativoDTO()
            {
                Id = info.Id,
                Subject = info.Subject,
                Body = info.Body,
                DoadoresFisicos = new List<DoadorDTO>(),
                 DoadoresJuridicos = new List<DoadorDTO>(),
                  Usuarios = info.Usuarios.Select(q => new UsuarioDTO()
                  {
                      Id = q.Usuario.Id,
                      Nome = q.Usuario.Nome,
                      Email = q.Usuario.Email
                  }).ToList(),
                  Voluntarios = info.Voluntarios.Select(q=> new VoluntarioDTO()
                  {
                       Id = q.Voluntario.Id,
                        Nome = q.Voluntario.Nome,
                        Email = q.Voluntario.Email
                  }   ).ToList(),
                Attachments = info.Attachments.Select(q=>new AttachmentDTO()
                {
                     Id=q.Id,
                       FileName = q.FileName
                }).ToList()

            };
            foreach(var d in info.Doadores)
            {
                if(d.Doador.GetType() == typeof(PessoaFisica))
                {
                    infoDto.DoadoresFisicos.Add(new DoadorDTO()
                    {
                        Id = d.CodDoador,
                        Email = d.Doador.Email,
                        NomeRazaoSocial = ((PessoaFisica)d.Doador).Nome
                    });
                }else
                {
                    infoDto.DoadoresJuridicos.Add(new DoadorDTO()
                    {
                        Id = d.CodDoador,
                        Email = d.Doador.Email,
                        NomeRazaoSocial = ((PessoaJuridica)d.Doador).RazaoSocial
                    });
                }
            }




            return infoDto;
        }

        
       

        // PUT api/values/5
        [HttpPut("{id}")]        
        public async Task<IActionResult> Put(int id, [FromBody]InformativoDTO infoDTO)
        {
            if (id != infoDTO.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);

            

                Usuario user = await _userManager.GetUserAsync(HttpContext.User);


                Informativo info = _context.Informativo
                  .Include(q => q.Attachments)
                  .Include(q => q.Doadores)
                  .ThenInclude(q => q.Doador)
                  .Include(q => q.Usuario)
                  .Include(q => q.Usuarios)
                  .ThenInclude(q => q.Usuario)
                  .Include(q => q.Voluntarios)
                  .ThenInclude(q => q.Voluntario)
                  .Where(q => q.IdUsuario == user.Id && q.Id == id)
                  .SingleOrDefault();
                if (info == null)
                    return new BadRequestResult();

                

                info.Subject = infoDTO.Subject;
                info.Body = infoDTO.Body;

                if (infoDTO.Usuarios == null)
                    infoDTO.Usuarios = new List<UsuarioDTO>();

                if (infoDTO.Voluntarios == null)
                    infoDTO.Voluntarios = new List<VoluntarioDTO>();

                if (infoDTO.DoadoresFisicos == null)
                    infoDTO.DoadoresFisicos = new List<DoadorDTO>();

                if (infoDTO.DoadoresJuridicos == null)
                    infoDTO.DoadoresJuridicos = new List<DoadorDTO>();

                if (info.Usuarios == null)
                    info.Usuarios = new List<InformativoUsuario>();

                if (info.Voluntarios == null)
                    info.Voluntarios = new List<InformativoVoluntario>();

                if (info.Doadores == null)
                    info.Doadores = new List<InformativoDoador>();

                var usersManter = from u in info.Usuarios
                                  join d in infoDTO.Usuarios on u.IdUsuario equals d.Id
                            select u;
                
                //para remover
                foreach(var u in info.Usuarios.Except(usersManter).ToArray())
                {
                    info.Usuarios.Remove(u);
                }

                var usersAdd = infoDTO.Usuarios.Where(q => !info.Usuarios.Any(u => u.IdUsuario == q.Id));
                //para adicionar
                foreach (var u in usersAdd)
                {
                    InformativoUsuario iu = new InformativoUsuario() { IdInformativo = info.Id, IdUsuario = u.Id };
                    info.Usuarios.Add(iu);
                }

                var doadoresManter = (from u in info.Doadores
                                join d in infoDTO.DoadoresFisicos on u.CodDoador equals d.Id
                                select u).Union(
                            from u in info.Doadores
                            join d in infoDTO.DoadoresJuridicos on u.CodDoador equals d.Id
                            select u);
                //para remover
                foreach (var u in info.Doadores.Except(doadoresManter).ToArray())
                {
                    info.Doadores.Remove(u);
                }

                var DoadoresAdd = infoDTO.DoadoresFisicos.Where(q => !info.Doadores.Any(u => u.CodDoador == q.Id))
                    .Union(infoDTO.DoadoresJuridicos.Where(q => !info.Doadores.Any(u => u.CodDoador == q.Id)));
                
                //para adicionar
                foreach (var u in DoadoresAdd)
                {
                    InformativoDoador ind = new InformativoDoador() { IdInformativo = info.Id,  CodDoador = u.Id };
                    info.Doadores.Add(ind);
                }

                var voluntariosManter = from u in info.Voluntarios
                            join d in infoDTO.Voluntarios on u.IdVoluntario equals d.Id
                            select u;
                //para remover
                foreach (var u in info.Voluntarios.Except(voluntariosManter).ToArray())
                {
                    info.Voluntarios.Remove(u);
                }

                //para adicionar
                var voluntarioAdd = infoDTO.Voluntarios.Where(q => !info.Voluntarios.Any(u => u.IdVoluntario == q.Id));
                foreach (var u in voluntarioAdd)
                {
                    InformativoVoluntario iv = new InformativoVoluntario() { IdInformativo = info.Id,  IdVoluntario = u.Id };
                    info.Voluntarios.Add(iv);
                }
            try
            {
                _context.SaveChanges();
            }
            catch {

            }

                return new ObjectResult(infoDTO);
            
        }

        // PUT api/values/5
        [HttpPost("Send/{id}")]
        public async Task<IActionResult> Send(int id)
        {
            Usuario user = await _userManager.GetUserAsync(HttpContext.User);
            Informativo info = _context.Informativo            
                  .Include(q => q.Attachments)
                  .Include(q => q.Doadores)
                  .ThenInclude(q => q.Doador)
                  .Include(q => q.Usuario)
                  .Include(q => q.Usuarios)
                  .ThenInclude(q => q.Usuario)
                  .Include(q => q.Voluntarios)
                  .ThenInclude(q => q.Voluntario)
                .SingleOrDefault(q => q.Id == id && q.IdUsuario == user.Id);
            if(info == null)
            {
                return new BadRequestResult();
            }

            //envia
            List<string> emails = (info.Doadores.Select(q => q.Doador.Email)
                .Union(info.Usuarios.Select(q => q.Usuario.Email))
                .Union(info.Voluntarios.Select(q => q.Voluntario.Email))).ToList();


            List<MimePart> attachments = new List<MimePart>();

            foreach (var a in info.Attachments)
            {
                attachments.Add(a.GetMimePart(_environment.WebRootPath));
            }

           _emailSender.SendEmailAsync(emails,info.Subject,info.Body, attachments);

            
            //apaga tudo
            try
            {
                foreach (var a in attachments)
                {
                    a.ContentObject.Stream.Dispose();

                }



                var attachmentsPath = Path.Combine(_environment.WebRootPath, "attachment", id.ToString());
                System.IO.Directory.Delete(attachmentsPath, true);

            }
            catch { }
                _context.Informativo.Remove(info);
                _context.SaveChanges();

                //cria um novo
                info = new Informativo()
                {
                    Subject = "",
                    Usuario = user,
                    Body = "",
                    Doadores = new List<InformativoDoador>(),
                    Usuarios = new List<InformativoUsuario>(),
                    Voluntarios = new List<InformativoVoluntario>()

                };
                _context.Informativo.Add(info);
                _context.SaveChanges();


                InformativoDTO infoDto = new InformativoDTO()
                {
                    Id = info.Id,
                    Subject = info.Subject,
                    Body = info.Body,
                    DoadoresFisicos = new List<DoadorDTO>(),
                    DoadoresJuridicos = new List<DoadorDTO>(),
                    Usuarios = new List<UsuarioDTO>(),
                    Voluntarios = new List<VoluntarioDTO>()

                };
                return new ObjectResult(infoDto);
            
            
            
        }
        [HttpPost("Attach/{id}")]
        public async Task<List<AttachmentDTO>> Post(int id)
        {
            var attachmentsPath = Path.Combine(_environment.WebRootPath, "attachment",id.ToString());
            if (!System.IO.Directory.Exists(attachmentsPath))
                System.IO.Directory.CreateDirectory(attachmentsPath);
            List<AttachmentDTO> atachs = new List<Models.DTOs.AttachmentDTO>();
            foreach (var file in Request.Form.Files) {


                if (file.Length > 0)
                {
                    Attachment attachment = new Attachment()
                    {
                        FileName = file.FileName,
                        Type = file.ContentType,
                        IdInformativo = id
                    };
                        //Path.Combine("attachment",id.ToString(), file.Name), file.GetType().Name, file.GetType().Name);
                    _context.Attachment.Add(attachment);
                    _context.SaveChanges();
                    atachs.Add(new AttachmentDTO()
                    {
                        Id = attachment.Id,
                        FileName = attachment.FileName
                    });
                    using (var fileStream = new FileStream(Path.Combine(_environment.WebRootPath, "attachment", id.ToString(),attachment.Id.ToString()), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    
                }
            }
             
            return atachs;
        }


        [HttpPost("Detach/{id}/{idAttachment}")]
        public  IActionResult Detach(int id,int idAttachment)
        {
            Attachment a = _context.Attachment.SingleOrDefault(q => q.Id == idAttachment && q.IdInformativo == id);
            if (a == null)
                return new BadRequestResult();

            var attachmentsPath = Path.Combine(_environment.WebRootPath, "attachment",id.ToString(), idAttachment.ToString());
            System.IO.File.Delete(attachmentsPath);
            _context.Attachment.Remove(a);
            _context.SaveChanges();

            return new NoContentResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Usuario user = await _userManager.GetUserAsync(HttpContext.User);
            Informativo info = _context.Informativo.Single(q => q.Id == id && q.IdUsuario == user.Id);
            var attachmentsPath = Path.Combine(_environment.WebRootPath, "attachment", id.ToString());
            try
            {
                System.IO.Directory.Delete(attachmentsPath);
            }
            catch { }
            _context.Informativo.Remove(info);
            

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
