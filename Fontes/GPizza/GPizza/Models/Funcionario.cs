using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GPizza.Models
{
    public class Funcionario
    {
        private int _fun_codigo;
        private string _fun_nome;
        private string _fun_nivel;
        private string _fun_usuario;
        private string _fun_senha;

        public Funcionario()
        {
            Fun_codigo = 0;
            Fun_nome = "";
            Fun_nivel = "";
            Fun_usuario = "";
            Fun_senha = "";
        }

        public Funcionario(int fun_codigo, string fun_nome, string fun_nivel, string fun_usuario, string fun_senha)
        {
            Fun_codigo = fun_codigo;
            Fun_nome = fun_nome;
            Fun_nivel = fun_nivel;
            Fun_usuario = fun_usuario;
            Fun_senha = fun_senha;
        }

        public int Fun_codigo { get => _fun_codigo; set => _fun_codigo = value; }
        public string Fun_nome { get => _fun_nome; set => _fun_nome = value; }
        public string Fun_nivel { get => _fun_nivel; set => _fun_nivel = value; }
        public string Fun_usuario { get => _fun_usuario; set => _fun_usuario = value; }
        public string Fun_senha { get => _fun_senha; set => _fun_senha = value; }


        public bool ValidarSenha(string usu_usuario, string usu_senha)
        {
            DAL.MySQLPersistencia bd =  new DAL.MySQLPersistencia();
            string sql = @"select count(*) from funcionario
                           where (fun_usuario = @fun_usuario) and (fun_senha = @fun_senha)";

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@fun_usuario", usu_usuario);
            parametros.Add("@fun_senha", usu_senha);

            Int64 qtde = (Int64)bd.ExecutarAgregacao(sql, parametros);

            return (qtde == 1);               
        }

        public bool Obter(string fun_usuario, string fun_senha)
        {
           

            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = @"select * from funcionario
                           where (fun_usuario = @fun_usuario) and (fun_senha = @fun_senha)";

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@fun_usuario", fun_usuario);
            parametros.Add("@fun_senha", fun_senha);

            DataTable dt = bd.ExecutarConsulta(sql, parametros);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                Fun_codigo = Convert.ToInt32(dt.Rows[0]["fun_codigo"]);
                Fun_nivel = dt.Rows[0]["fun_nivel"].ToString();
                Fun_nome = dt.Rows[0]["fun_nome"].ToString();
                Fun_senha = dt.Rows[0]["fun_senha"].ToString();
                Fun_usuario = dt.Rows[0]["fun_usuario"].ToString();

                return true;
            }

            return false;
        }

        public List<Funcionario> Pesquisar(string nome)
        {
            List<Funcionario> funcs = new List<Funcionario>();

            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from funcionario where fun_nome like @fun_nome";

            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@fun_nome", nome + "%");

            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.

            foreach (DataRow linha in dt.Rows)
            {
                Funcionario f = new Funcionario();
                f.Fun_codigo = Convert.ToInt32(linha["fun_codigo"]);
                f.Fun_nome = linha["fun_nome"].ToString();
                f.Fun_nivel = linha["fun_nivel"].ToString();
                f.Fun_usuario = linha["fun_usuario"].ToString();
                f.Fun_senha = linha["fun_senha"].ToString();

                funcs.Add(f);
            }

            return funcs;
        }

        public void Obter(int fun_codigo)
        {       
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from funcionario where fun_codigo = @fun_codigo";

            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@fun_codigo", fun_codigo);

            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                Fun_codigo = Convert.ToInt32(dt.Rows[0]["fun_codigo"]);
                Fun_nivel = dt.Rows[0]["fun_nivel"].ToString();
                Fun_nome = dt.Rows[0]["fun_nome"].ToString();
                Fun_senha = dt.Rows[0]["fun_senha"].ToString();
                Fun_usuario = dt.Rows[0]["fun_usuario"].ToString();
            }
        }

        public Funcionario getFuncionario(int fun_codigo)
        {
            Funcionario fun = null;
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from funcionario where fun_codigo = @fun_codigo";

            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@fun_codigo", fun_codigo);

            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                fun = new Funcionario();
                fun.Fun_codigo = Convert.ToInt32(dt.Rows[0]["fun_codigo"]);
                fun.Fun_nivel = dt.Rows[0]["fun_nivel"].ToString();
                fun.Fun_nome = dt.Rows[0]["fun_nome"].ToString();
                fun.Fun_senha = dt.Rows[0]["fun_senha"].ToString();
                fun.Fun_usuario = dt.Rows[0]["fun_usuario"].ToString();
            }
            return fun;
        }

        public bool Gravar(Funcionario f, out string msg)
        {

            msg = "";

            if (f.Fun_nome.Length < 10)
            {
                msg = "Nome muito pequeno.";
                return false;
            }

            if (f.Fun_nivel == "") 
            {
                msg = "Nível de Acesso não informado.";
                return false;
            }

            if ((f.Fun_nivel != "1") && (f.Fun_nivel != "2"))
            {
                msg = "Nível de Acesso inválido.";
                return false;
            }

            if (f.Fun_usuario.Length < 5)
            {
                msg = "Informe um Usuário com no mínimo 5 caracteres.";
                return false;
            }

            if (f.Fun_senha.Length < 5)
            {
                msg = "Informe uma Senha com no mínimo 5 caracteres.";
                return false;
            }
            
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();

            string sql = "";
            Dictionary<string, object> ps = new Dictionary<string, object>();

            if (f.Fun_codigo == 0)
            {
                sql = @"insert into funcionario (fun_nome, fun_nivel, fun_usuario, fun_senha) 
                                         values (@fun_nome, @fun_nivel, @fun_usuario, @fun_senha)";
            }
            else
            {
                sql = @"update funcionario 
                         set  fun_nome    = @fun_nome,
                              fun_nivel   = @fun_nivel,
                              fun_usuario = @fun_usuario,
                              fun_senha   = @fun_senha
                         where fun_codigo = @fun_codigo";

                ps.Add("@fun_codigo", f.Fun_codigo);
            }
            ps.Add("@fun_nome", f.Fun_nome);
            ps.Add("@fun_nivel", f._fun_nivel);
            ps.Add("@fun_usuario", f.Fun_usuario);
            ps.Add("@fun_senha", f.Fun_senha);

            int r = bd.ExecutarComando(sql, ps);
            msg = bd.MsgErro;
            return r == 1;
        }

        public bool Excluir(int fun_codigo)
        {
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "delete from funcionario where fun_codigo = @fun_codigo";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@fun_codigo", fun_codigo);

            int linhasAfetadas = bd.ExecutarComando(sql, ps);
            return linhasAfetadas > 0;
        }




    }


}
