using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPizza.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult IndexMenu()
        {
            //return RedirectToRoute("Shared", "_Layout");
            return View("_Layout");
        }

        [HttpPost]
        public JsonResult Logar([FromBody]Dictionary<string, string> dados)
        {
            Models.Funcionario f = new Models.Funcionario();
            if (f.Obter(dados["fun_usuario"], dados["fun_senha"]))
            {
                HttpContext.Session.SetString("fun_codigo", f.Fun_codigo.ToString());
                HttpContext.Session.SetString("fun_nome", f.Fun_nome);
                HttpContext.Session.SetString("fun_nivel", f.Fun_nivel);
            }

            return Json(new { operacao = (f!=null)});
        }
        
        
    }
}