using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GPizza.Controllers
{
    public class CidadeController : Controller
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
            Models.Cidade c = new Models.Cidade();
            if (dados["cid_codigo"] != "")
                c.Cid_codigo = Convert.ToInt32(dados["cid_codigo"]);
            else
                c.Cid_codigo = 0;
            c.Cid_nome = dados["cid_nome"];
            c.Cid_uf = dados["cid_uf"];
            bool ok = c.Gravar(c, out msg);
            var obj = new
            {
                cid_codigo = c.Cid_codigo,
                operacao = ok,
                msg = msg
            };       
            return Json(obj);
        }

        public IActionResult IndexPesquisar()
        {
            return View();
        }

        public JsonResult Pesquisar(string cid_nome)
        {
            Models.Cidade c = new Models.Cidade();
            List<Models.Cidade> cidades = c.Pesquisar(cid_nome);
            return Json(cidades);

        }

        public JsonResult Obter(int id)
        {
            Models.Cidade c = new Models.Cidade();
            c.Obter(id);
            var dados = new
            {
                cid_codigo = c.Cid_codigo,
                cid_nome = c.Cid_nome,
                cid_uf = c.Cid_uf
            };
            return Json(dados);
        }

        public JsonResult Excluir([FromBody] Dictionary<string, string> dados)
        {
            string msg = "";
            int cid_codigo = Convert.ToInt32(dados["cid_codigo"]);
            Models.Cidade c = new Models.Cidade();
            bool ok = c.Excluir(cid_codigo);

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