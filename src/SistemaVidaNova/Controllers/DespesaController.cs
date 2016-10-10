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
    public class DespesaController : Controller
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

        public IActionResult Associacao(int id)
        {

            return View();
        }
        public IActionResult Sopa(int id)
        {

            return View();
        }
        public IActionResult Favorecido(int id)
        {

            return View();
        }

    }
}
