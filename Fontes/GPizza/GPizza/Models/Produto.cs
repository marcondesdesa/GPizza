using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GPizza.Models
{
    public class Produto
    {
        private int _pro_codigo;
        private string _pro_descricao;
        private double _pro_preco;
        private double _pro_estoque;
        private int _pro_tipo;
        private string _pro_observacao;

        public Produto()
        {
            Pro_codigo = 0;
            Pro_descricao = "";
            Pro_preco = 0;
            Pro_estoque = 0;
            Pro_tipo = 0;
            Pro_observacao = "";
        }

        public Produto(int pro_codigo, string pro_descricao, double pro_preco, double pro_estoque, int pro_tipo, string pro_observacao)
        {
            Pro_codigo = pro_codigo;
            Pro_descricao = pro_descricao;
            Pro_preco = pro_preco;
            Pro_estoque = pro_estoque;
            Pro_tipo = pro_tipo;
            Pro_observacao = pro_observacao;
        }

        public int Pro_codigo { get => _pro_codigo; set => _pro_codigo = value; }
        public string Pro_descricao { get => _pro_descricao; set => _pro_descricao = value; }
        public double Pro_preco { get => _pro_preco; set => _pro_preco = value; }
        public double Pro_estoque { get => _pro_estoque; set => _pro_estoque = value; }
        public int Pro_tipo { get => _pro_tipo; set => _pro_tipo = value; }
        public string Pro_observacao { get => _pro_observacao; set => _pro_observacao = value; }
      
        public List<Produto> Pesquisar(string pro_descricao)
        {
            List<Produto> prods = new List<Produto>();

            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from produto where pro_descricao like @pro_descricao";

            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@pro_descricao", pro_descricao + "%");

            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
           
            foreach (DataRow linha in dt.Rows)
            {
                Produto p = new Produto();
                p.Pro_codigo = Convert.ToInt32(linha["pro_codigo"]);
                p.Pro_descricao = linha["pro_descricao"].ToString();
                p.Pro_preco = Convert.ToDouble(linha["pro_preco"].ToString());
                p.Pro_estoque = Convert.ToDouble(linha["pro_estoque"].ToString());
                p.Pro_tipo = Convert.ToInt32(linha["pro_tipo"].ToString());
                p.Pro_observacao = linha["pro_observacao"].ToString();
                prods.Add(p);
            }

            return prods;
        }

        public void Obter(int pro_codigo)
        {
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from produto where pro_codigo = @pro_codigo";

            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@pro_codigo", pro_codigo);

            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                Pro_codigo = Convert.ToInt32(dt.Rows[0]["pro_codigo"]);
                Pro_descricao = dt.Rows[0]["pro_descricao"].ToString();
                Pro_preco = Convert.ToDouble(dt.Rows[0]["pro_preco"].ToString());
                Pro_estoque = Convert.ToDouble(dt.Rows[0]["pro_estoque"].ToString());
                Pro_tipo = Convert.ToInt32(dt.Rows[0]["pro_tipo"].ToString());
                Pro_observacao = dt.Rows[0]["pro_observacao"].ToString();             
            }
        }

        public Produto getProduto(int pro_codigo)
        {
            Produto pro = null;
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from produto where pro_codigo = @pro_codigo";

            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@pro_codigo", pro_codigo);

            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                pro = new Produto();
                pro.Pro_codigo = Convert.ToInt32(dt.Rows[0]["pro_codigo"]);
                pro.Pro_descricao = dt.Rows[0]["pro_descricao"].ToString();
                pro.Pro_preco = Convert.ToDouble(dt.Rows[0]["pro_preco"].ToString());
                pro.Pro_estoque = Convert.ToDouble(dt.Rows[0]["pro_estoque"].ToString());
                pro.Pro_tipo = Convert.ToInt32(dt.Rows[0]["pro_tipo"].ToString());
                pro.Pro_observacao = dt.Rows[0]["pro_observacao"].ToString();
            }
            return pro;
        }

        public bool Gravar(Produto p, out string msg)
        {
            msg = "";

            if (p.Pro_descricao.Length < 10)
            {
                msg = "Nome muito pequeno.";
                return false;
            }

            if (p.Pro_preco <= 0)
            {
                msg = "Informe o Preço do Produto";
                return false;
            }

            if (p.Pro_tipo <= 0)
            {
                msg = "Informe o Tipo de Produto";
                return false;
            }

            if ((p.Pro_tipo != 1) && (p.Pro_tipo != 2) && (p.Pro_tipo != 3))
            {
                msg = "Tipo de Produto inválido.";
                return false;
            }
                   
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();

            string sql = "";
            Dictionary<string, object> ps = new Dictionary<string, object>();

            if (p.Pro_codigo == 0)
            {
                sql = @"insert into produto (pro_descricao, pro_preco, pro_estoque, pro_tipo,pro_observacao) 
                                     values (@pro_descricao,@pro_preco,@pro_estoque,@pro_tipo,@pro_observacao)";
            }
            else
            {
                sql = @"update produto 
                         set  pro_descricao  = @pro_descricao,
                              pro_preco      = @pro_preco,
                              pro_estoque    = @pro_estoque,
                              pro_tipo       = @pro_tipo,
                              pro_observacao = @pro_observacao
                         where pro_codigo = @pro_codigo";
                ps.Add("@pro_codigo", p.Pro_codigo);
            }
            ps.Add("@pro_descricao", p.Pro_descricao);
            ps.Add("@pro_preco", p.Pro_preco);
            ps.Add("@pro_estoque", p.Pro_estoque);
            ps.Add("@pro_tipo", p.Pro_tipo);
            ps.Add("@pro_observacao", p.Pro_observacao);

            int r = bd.ExecutarComando(sql, ps);
            msg = bd.MsgErro;
            return r == 1;
        }

        public bool Excluir(int pro_codigo)
        {
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "delete from produto where pro_codigo = @pro_codigo";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@pro_codigo", pro_codigo);

            int linhasAfetadas = bd.ExecutarComando(sql, ps);          
            return linhasAfetadas > 0;
        }

    }
}
