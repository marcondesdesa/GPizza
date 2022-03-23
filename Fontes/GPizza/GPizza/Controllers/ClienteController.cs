using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static GPizza.Funcoes;

namespace GPizza.Controllers
{
    public class ClienteController : Controller
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
            Models.Cliente c = new Models.Cliente();
            c.Cli_codigo = getInt((string)dados["cli_codigo"]);
            //if (dados["cli_codigo"] != "")
            //    c.Cli_codigo = Convert.ToInt32(dados["cli_codigo"]);
            //else
            //    c.Cli_codigo = 0; 
            Models.Cidade cidade = new Models.Cidade();
            c.Cli_nome = dados["cli_nome"];
            c.Cli_endereco = dados["cli_endereco"];
            c.Cli_numero = getInt((string)dados["cli_numero"]);             
            c.Cli_bairro = dados["cli_bairro"];       
            cidade.getCidade(getInt((string)dados["cid_codigo"]));
            c.Cidade = cidade;
            c.Cli_cep = dados["cli_cep"];
            c.Cli_telefone = dados["cli_telefone"];
            c.Cli_celular = dados["cli_celular"];
            c.Cli_cpf = dados["cli_cpf"];
            c.Cli_rg = dados["cli_rg"];
            c.Cli_dt_nascimento = getDateTime((string)dados["cli_dt_nascimento"]);
            c.Cli_email = dados["cli_email"];
            c.Cli_observacao = dados["cli_observacao"];

            bool ok = c.Gravar(c, out msg);
            var obj = new
            {
                cli_codigo = c.Cli_codigo,
                operacao = ok,
                msg = msg
            };
            return Json(obj);
        }

        public IActionResult IndexPesquisar()
        {
            return View();
        }

        public JsonResult Pesquisar(string cli_nome)
        {
            Models.Cliente c = new Models.Cliente();
            List<Models.Cliente> clientes = c.Pesquisar(cli_nome);
            return Json(clientes);

        }

        public JsonResult Obter(int id)
        {
            Models.Cliente c = new Models.Cliente();
            c.Obter(id);
            var dados = new
            {
                cli_codigo = c.Cli_codigo,
                cli_nome = c.Cli_nome,
                cli_endereco = c.Cli_endereco,
                cli_numero = c.Cli_numero,
                cli_bairro = c.Cli_bairro,
                cidade = new
                        {
                            cid_codigo = c.Cidade.Cid_codigo,
                            cid_nome = c.Cidade.Cid_nome,
                            cid_uf = c.Cidade.Cid_uf
                        },         
                cli_cep = c.Cli_cep,
                cli_telefone = c.Cli_telefone,
                cli_celular = c.Cli_celular,
                cli_cpf = c.Cli_cpf,
                cli_rg = c.Cli_rg,
                cli_dt_nascimento = c.Cli_dt_nascimento, //System.DateTime.Parse(c.Cli_dt_nascimento.ToString("dd/MM/yyyy")),
                cli_email = c.Cli_email,
                cli_observacao = c.Cli_observacao
            };
            return Json(dados);
        }

        public JsonResult Excluir([FromBody] Dictionary<string, string> dados)
        {
            string msg = "";
            int cli_codigo = Convert.ToInt32(dados["cli_codigo"]);
            Models.Cliente c = new Models.Cliente();
            bool ok = c.Excluir(cli_codigo);

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