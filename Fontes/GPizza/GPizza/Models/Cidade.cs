using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GPizza.Models
{
    public class Cidade
    {
        private int cid_codigo;
        private string cid_nome;
        private string cid_uf;

        public Cidade()
        {
            this.cid_codigo = 0;
            this.cid_nome = "";
            this.cid_uf = "";
        }

        public Cidade(int cid_codigo, string cid_nome, string cid_uf)
        {
            this.cid_codigo = cid_codigo;
            this.cid_nome = cid_nome;
            this.cid_uf = cid_uf;
        }

        public int Cid_codigo { get => cid_codigo; set => cid_codigo = value; }
        public string Cid_nome { get => cid_nome; set => cid_nome = value; }
        public string Cid_uf { get => cid_uf; set => cid_uf = value; }


        public List<Cidade> Pesquisar(string cid_nome)
        {
            List<Cidade> cidades = new List<Cidade>();
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from cidade where cid_nome like @cid_nome order by cid_nome";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cid_nome", cid_nome + "%");
            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            foreach (DataRow linha in dt.Rows)
            {
                Cidade c = new Cidade();
                c.Cid_codigo = Convert.ToInt32(linha["cid_codigo"]);
                c.Cid_nome = linha["cid_nome"].ToString();
                c.Cid_uf = linha["cid_uf"].ToString();
                cidades.Add(c);
            }
            return cidades;
        }

        public void Obter(int cid_codigo)
        {
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from cidade where cid_codigo = @cid_codigo";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cid_codigo", cid_codigo);
            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                Cid_codigo = Convert.ToInt32(dt.Rows[0]["cid_codigo"]);
                Cid_nome = dt.Rows[0]["cid_nome"].ToString();
                Cid_uf = dt.Rows[0]["cid_uf"].ToString();
            }
        }

        public void getCidade(int cid_codigo)
        {
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from cidade where cid_codigo = @cid_codigo";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cid_codigo", cid_codigo);
            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                Cid_codigo = Convert.ToInt32(dt.Rows[0]["cid_codigo"]);
                Cid_nome = dt.Rows[0]["cid_nome"].ToString();
                Cid_uf = dt.Rows[0]["cid_uf"].ToString();              
            }
        }

        public Cidade getObjCidade(int cid_codigo)
        {
            Cidade cid = null;
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from cidade where cid_codigo = @cid_codigo";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cid_codigo", cid_codigo);
            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                cid = new Cidade(Convert.ToInt32(dt.Rows[0]["cid_codigo"]), dt.Rows[0]["cid_nome"].ToString(), dt.Rows[0]["cid_uf"].ToString());              
            }
            return cid;
        }

        public bool Gravar(Cidade c, out string msg)
        {
            msg = "";
            if (c.Cid_nome.Length < 3)
            {
                msg = "Nome muito pequeno.";
                return false;
            }
            if (c.Cid_uf.Length != 2)
            {
                msg = "UF deve conter dois caracteres.";
                return false;
            }  
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            if (c.Cid_codigo == 0)
            {
                sql = @"insert into cidade (cid_nome, cid_uf) 
                                    values (@cid_nome, @cid_uf)";
            }
            else
            {
                sql = @"update cidade 
                         set  cid_nome    = @cid_nome,
                              cid_uf   = @cid_uf
                         where cid_codigo = @cid_codigo";
                ps.Add("@cid_codigo", c.Cid_codigo);
            }
            ps.Add("@cid_nome", c.Cid_nome);
            ps.Add("@cid_uf", c.Cid_uf);        
            int r = bd.ExecutarComando(sql, ps);
            msg = bd.MsgErro;
            return r == 1;
        }

        public bool Excluir(int cid_codigo)
        {
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "delete from cidade where cid_codigo = @cid_codigo";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cid_codigo", cid_codigo);
            int linhasAfetadas = bd.ExecutarComando(sql, ps);
            return linhasAfetadas > 0;
        }
    }
}
