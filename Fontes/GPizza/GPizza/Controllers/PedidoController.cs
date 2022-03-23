using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static GPizza.Funcoes;
using Microsoft.AspNetCore.Authorization;
//Importando package para session
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GPizza.Controllers
{
    public class PedidoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult PesquisarProduto(string termo)
        {

            Models.Produto p = new Models.Produto();
            List<Models.Produto> lista = p.Pesquisar(termo);
            return Json(lista);
        }

        [Filtro]
        [HttpPost]
        public JsonResult Gravar([FromBody] Dictionary<string, object> dados)
        {
            string msg = "";
            bool operacao = false;           
            Models.Pedido p = new Models.Pedido();
            p.Ped_codigo = Convert.ToInt32(dados["ped_codigo"]);        
            p.Cliente = new Models.Cliente().getCliente(Convert.ToInt32(dados["cli_codigo"]));
            //object objFun = (Models.Funcionario)dados["funcionario"];
            if (HttpContext.Session.GetString("fun_codigo")!=null)
                p.Funcionario = new Models.Funcionario().getFuncionario(Convert.ToInt32(HttpContext.Session.GetString("fun_codigo")));  
            p.Ped_data = getDateTime((string)dados["ped_data"]);         
            p.Ped_desconto = Convert.ToDouble(dados["ped_desconto"]);
            p.Ped_valor_total = Convert.ToDouble(dados["ped_valor_total"]);
            p.Ped_observacao = dados["ped_observacao"].ToString();

            Newtonsoft.Json.Linq.JArray items = (Newtonsoft.Json.Linq.JArray)dados["items"];

            p.Itens = new List<Models.PedidoItem>();

            foreach (Newtonsoft.Json.Linq.JObject item in items)
            {
                Models.PedidoItem i = new Models.PedidoItem();
                i.Produto = new Models.Produto();
                i.Produto.Pro_codigo = item.Value<int>("pro_codigo");
                i.Pi_quant = item.Value<int>("pi_quant");
                i.Pi_valor_unit = item.Value<double>("pi_valor_unit");
                i.Pi_valor_tot = item.Value<double>("pi_valor_tot");
                p.Itens.Add(i);
            }
            operacao = p.Gravar(out msg);

            return Json(new
            {
                ped_codigo = p.Ped_codigo,
                operacao = operacao,
                msg = msg
            }); ;
        }

        public IActionResult IndexPesquisar()
        {
            return View();
        }

        public JsonResult Pesquisar(string cli_nome)
        {
            Models.Pedido c = new Models.Pedido();
            List<Models.Pedido> pedidos = c.Pesquisar(cli_nome);
            return Json(pedidos);

        }

        public JsonResult Obter(int id)
        {
            Models.Pedido p = new Models.Pedido();
            p.Obter(id);
                         
            var dados = new
            {
                ped_codigo = p.Ped_codigo,
                ped_data = p.Ped_data,
                funcionario = new
                {
                    fun_codigo = p.Funcionario.Fun_codigo,
                    fun_nome = p.Funcionario.Fun_nome,
                    fun_nivel = p.Funcionario.Fun_nivel,
                    fun_usuario = p.Funcionario.Fun_usuario
                },
                cliente = new
                {
                    cli_codigo = p.Cliente.Cli_codigo,
                    cli_nome = p.Cliente.Cli_nome,
                    cli_endereco = p.Cliente.Cli_endereco,
                    cli_numero = p.Cliente.Cli_numero,
                    cli_bairro = p.Cliente.Cli_bairro,
                    cidade = new
                    {
                        cid_codigo = p.Cliente.Cidade.Cid_codigo,
                        cid_nome = p.Cliente.Cidade.Cid_nome,
                        cid_uf = p.Cliente.Cidade.Cid_uf
                    },
                    cli_cep = p.Cliente.Cli_cep,
                    cli_telefone = p.Cliente.Cli_telefone,
                    cli_celular = p.Cliente.Cli_celular,
                    cli_cpf = p.Cliente.Cli_cpf,
                    cli_rg = p.Cliente.Cli_rg,
                    cli_dt_nascimento = p.Cliente.Cli_dt_nascimento,
                    cli_email = p.Cliente.Cli_email,
                    cli_observacao = p.Cliente.Cli_observacao
                },
                itensPed = new List<object>(),
                ped_desconto = p.Ped_desconto,
                ped_valor_total = p.Ped_valor_total,
                ped_observacao = p.Ped_observacao
            
            };

            foreach (Models.PedidoItem item in p.Itens)
            {
                var produtoItem = new
                {
                    pro_codigo = item.Produto.Pro_codigo,
                    pro_descricao = item.Produto.Pro_descricao,
                    pro_preco = item.Produto.Pro_preco,
                    pro_estoque = item.Produto.Pro_estoque,
                    pro_tipo = item.Produto.Pro_tipo,
                    pro_observacao = item.Produto.Pro_observacao
                };

                dados.itensPed.Add(new {
                    produto = produtoItem,
                    pi_quant = item.Pi_quant,
                    pi_valor_unit = item.Pi_valor_unit,
                    pi_valor_tot = item.Pi_valor_tot
                });                           
            }

            return Json(dados);
        }

    }
}