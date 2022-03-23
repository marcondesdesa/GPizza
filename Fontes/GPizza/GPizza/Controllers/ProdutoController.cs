using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GPizza.Controllers
{
    public class ProdutoController : Controller
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

            Models.Produto p = new Models.Produto();
            if (dados["pro_codigo"] != "")
                p.Pro_codigo = Convert.ToInt32(dados["pro_codigo"]);
            else
                p.Pro_codigo = 0;
            p.Pro_descricao = dados["pro_descricao"];           
            p.Pro_preco = Convert.ToDouble(dados["pro_preco"]);
            p.Pro_estoque = Convert.ToDouble(dados["pro_estoque"]);
            p.Pro_tipo = Convert.ToInt32(dados["pro_tipo"]);
            p.Pro_observacao = dados["pro_observacao"];
            bool ok = p.Gravar(p, out msg);
               
            var obj = new
            {
                pro_codigo = p.Pro_codigo,
                operacao = ok,
                msg = msg
            };         

            return Json(obj);
        }

        public IActionResult IndexPesquisar()
        {
            return View();
        }

        public JsonResult Pesquisar(string pro_descricao)
        {
            Models.Produto p = new Models.Produto();
            List<Models.Produto> prods = p.Pesquisar(pro_descricao);
            return Json(prods);

        }

        public JsonResult Obter(int id)
        {
            Models.Produto p = new Models.Produto();
            p.Obter(id);
            
            var dados = new
            {
                pro_codigo = p.Pro_codigo,
                pro_descricao = p.Pro_descricao,
                pro_preco = p.Pro_preco,
                pro_estoque = p.Pro_estoque,
                pro_tipo = p.Pro_tipo,
                pro_observacao = p.Pro_observacao
            };

            return Json(dados);

        }

        public JsonResult Excluir([FromBody] Dictionary<string, string> dados)
        {
            string msg = "";
            int pro_codigo = Convert.ToInt32(dados["pro_codigo"]);
            Models.Produto p = new Models.Produto();
            bool ok = p.Excluir(pro_codigo);

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