using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPizza.Controllers
{
    
    public class FuncionarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [Filtro]
        public JsonResult Gravar([FromBody] Dictionary<string, string> dados)
        {
            //Deserializando os dados em um dictionary

            string msg = "";

            Models.Funcionario f = new Models.Funcionario();
            if (dados["fun_codigo"] != "")
                f.Fun_codigo = Convert.ToInt32(dados["fun_codigo"]);
            else
                f.Fun_codigo = 0;
            f.Fun_nome = dados["fun_nome"];           
            f.Fun_nivel = dados["fun_nivel"];
            f.Fun_usuario = dados["fun_usuario"];
            f.Fun_senha = dados["fun_senha"];
            bool ok = f.Gravar(f, out msg);

            var obj = new
            {
                fun_codigo = f.Fun_codigo,
                operacao = ok,
                msg = msg
            };         

            return Json(obj);
        }


        public IActionResult IndexPesquisar()
        {
            return View();
        }

        public JsonResult Pesquisar(string nome)
        {

            Models.Funcionario f = new Models.Funcionario();
            List<Models.Funcionario> funcs = f.Pesquisar(nome);

            return Json(funcs);

        }

        public JsonResult Obter(int id)
        {
            Models.Funcionario f = new Models.Funcionario();
            f.Obter(id);
            
            var dados = new
            {
                fun_codigo = f.Fun_codigo,
                fun_nome = f.Fun_nome,
                fun_nivel = f.Fun_nivel,
                fun_usuario = f.Fun_usuario,
                fun_senha = f.Fun_senha
            };

            return Json(dados);

        }

        public JsonResult Excluir([FromBody] Dictionary<string, string> dados)
        {
            string msg = "";
            int fun_codigo = Convert.ToInt32(dados["fun_codigo"]);
            Models.Funcionario f = new Models.Funcionario();
            bool ok = f.Excluir(fun_codigo);

            if (ok)
                msg = "Excluido com sucesso.";
            else
                msg = "Não foi possível excluir o registro.";

            return Json(new
            {
                operacao = ok,
                msg = msg
            });

        }



    }
}