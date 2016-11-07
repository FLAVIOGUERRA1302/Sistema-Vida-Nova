using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Controllers
{
    [Authorize]
    public class DoacaoController : Controller
    {
        // GET: /<controller>/
        

        public IActionResult Index(int id)
        {
            return View();
        }

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Visualizar(int id)
        {

            return View();
        }

        public IActionResult Editar(int id)
        {

            return View();
        }

        [Route("Doacao/Dinheiro/{a}/{b}")]
        public IActionResult Dinheiro(string a, string b)
        {

            return View();
        }
        
        [Route("Doacao/Sopa/{a}/{b}")]
        public IActionResult Sopa(string a, string b)
        {

            return View();
        }
        [Route("Doacao/Obejto/{a}/{b}")]
        public IActionResult Obejto(string a, string b, string c)
        {

            return View();
        }
        [Route("Doacao/RelatorioDinheiro/{a}/{b}")]
        public IActionResult RelatorioDinheiro(string a, string b)
        {

            return View();
        }


    }
}
