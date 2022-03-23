using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static GPizza.Funcoes;

namespace GPizza.Models
{
    public class Cliente
    {
        private int cli_codigo;
        private string cli_nome;
        private string cli_endereco;
        private int cli_numero;
        private string cli_bairro;
        private Cidade cidade;
        private string cli_cep;
        private string cli_telefone;
        private string cli_celular;
        private DateTime cli_dt_nascimento;
        private string cli_cpf;
        private string cli_rg;
        private string cli_email;
        private string cli_observacao;

       

        public Cliente()
        {
            this.Cli_codigo = 0;
            this.Cli_nome = "";
            this.Cli_endereco = "";
            this.Cli_numero = 0;
            this.Cli_bairro = "";
            this.Cidade = null;
            this.Cli_cep = "";
            this.Cli_telefone = "";
            this.Cli_celular = "";
            this.Cli_dt_nascimento = DateTime.Now;
            this.Cli_cpf = "";
            this.Cli_rg = "";
            this.Cli_email = "";
            this.Cli_observacao = "";
        }

        public Cliente(int cli_codigo, string cli_nome, string cli_endereco, int cli_numero, string cli_bairro, Cidade cidade, string cli_cep, string cli_telefone, string cli_celular, DateTime cli_dt_nascimento, string cli_cpf, string cli_rg, string cli_email, string cli_observacao)
        {
            this.Cli_codigo = cli_codigo;
            this.Cli_nome = cli_nome;
            this.Cli_endereco = cli_endereco;
            this.Cli_numero = cli_numero;
            this.Cli_bairro = cli_bairro;
            this.Cidade = cidade;
            this.Cli_cep = cli_cep;
            this.Cli_telefone = cli_telefone;
            this.Cli_celular = cli_celular;
            this.Cli_dt_nascimento = cli_dt_nascimento;
            this.Cli_cpf = cli_cpf;
            this.Cli_rg = cli_rg;
            this.Cli_email = cli_email;
            this.Cli_observacao = cli_observacao;
        }

        public int Cli_codigo { get => cli_codigo; set => cli_codigo = value; }
        public string Cli_nome { get => cli_nome; set => cli_nome = value; }
        public string Cli_endereco { get => cli_endereco; set => cli_endereco = value; }
        public int Cli_numero { get => cli_numero; set => cli_numero = value; }
        public string Cli_bairro { get => cli_bairro; set => cli_bairro = value; }
        public Cidade Cidade { get => cidade; set => cidade = value; }
        public string Cli_cep { get => cli_cep; set => cli_cep = value; }
        public string Cli_telefone { get => cli_telefone; set => cli_telefone = value; }
        public string Cli_celular { get => cli_celular; set => cli_celular = value; }
        public DateTime Cli_dt_nascimento { get => cli_dt_nascimento; set => cli_dt_nascimento = value; }
        public string Cli_cpf { get => cli_cpf; set => cli_cpf = value; }
        public string Cli_rg { get => cli_rg; set => cli_rg = value; }
        public string Cli_email { get => cli_email; set => cli_email = value; }
        public string Cli_observacao { get => cli_observacao; set => cli_observacao = value; }


        public List<Cliente> Pesquisar(string cli_nome)
        {
            List<Cliente> clientes = new List<Cliente>();
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from cliente where cli_nome like @cli_nome order by cli_nome";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cli_nome", cli_nome + "%");
            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            foreach (DataRow linha in dt.Rows)
            {
                Cidade cid = new Cidade();
                Cliente c = new Cliente();
                c.Cli_codigo = Convert.ToInt32(linha["cli_codigo"]);
                c.Cli_nome = linha["cli_nome"].ToString();
                c.Cli_endereco = linha["cli_endereco"].ToString();
                c.Cli_numero = Convert.ToInt32(linha["cli_numero"]);
                c.Cli_bairro = linha["cli_bairro"].ToString();
                cid.getCidade(Convert.ToInt32(linha["cid_codigo"]));
                c.Cidade = cid;
                c.Cli_cep = linha["cli_cep"].ToString();
                c.Cli_telefone = linha["cli_telefone"].ToString();
                c.Cli_celular = linha["cli_celular"].ToString();
                c.Cli_dt_nascimento = DateTime.Parse(linha["cli_dt_nascimento"].ToString());
                c.Cli_cpf = linha["cli_cpf"].ToString();
                c.Cli_rg = linha["cli_rg"].ToString();
                c.Cli_email = linha["cli_email"].ToString();
                c.Cli_observacao = linha["cli_observacao"].ToString();
                clientes.Add(c);
            }
            return clientes;
        }

        public void Obter(int cli_codigo)
        {
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from cliente where cli_codigo = @cli_codigo";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cli_codigo", cli_codigo);
            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                Cidade cid = new Cidade();
                Cli_codigo = getInt(dt.Rows[0]["cli_codigo"].ToString());
                Cli_nome = dt.Rows[0]["cli_nome"].ToString();
                Cli_endereco = dt.Rows[0]["cli_endereco"].ToString();
                Cli_numero = getInt(dt.Rows[0]["cli_numero"].ToString());
                Cli_bairro = dt.Rows[0]["cli_bairro"].ToString();
                cid.getCidade(getInt(dt.Rows[0]["cid_codigo"].ToString()));
                Cidade = cid;
                Cli_cep = dt.Rows[0]["cli_cep"].ToString();
                Cli_telefone = dt.Rows[0]["cli_telefone"].ToString();
                Cli_celular = dt.Rows[0]["cli_celular"].ToString();
                Cli_dt_nascimento = DateTime.Parse(dt.Rows[0]["cli_dt_nascimento"].ToString());
                Cli_cpf = dt.Rows[0]["cli_cpf"].ToString();
                Cli_rg = dt.Rows[0]["cli_rg"].ToString();
                Cli_email = dt.Rows[0]["cli_email"].ToString();
                Cli_observacao = dt.Rows[0]["cli_observacao"].ToString();
            }
        }

        public Cliente getCliente(int cli_codigo)
        {
            Cliente cli = null;
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from cliente where cli_codigo = @cli_codigo";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cli_codigo", cli_codigo);
            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                cli = new Cliente();
                Cidade cid = new Cidade();
                cli.Cli_codigo = getInt(dt.Rows[0]["cli_codigo"].ToString());
                cli.Cli_nome = dt.Rows[0]["cli_nome"].ToString();
                cli.Cli_endereco = dt.Rows[0]["cli_endereco"].ToString();
                cli.Cli_numero = getInt(dt.Rows[0]["cli_numero"].ToString());
                cli.Cli_bairro = dt.Rows[0]["cli_bairro"].ToString();
                cid.getCidade(getInt(dt.Rows[0]["cid_codigo"].ToString()));
                cli.Cidade = cid;
                cli.Cli_cep = dt.Rows[0]["cli_cep"].ToString();
                cli.Cli_telefone = dt.Rows[0]["cli_telefone"].ToString();
                cli.Cli_celular = dt.Rows[0]["cli_celular"].ToString();
                cli.Cli_dt_nascimento = DateTime.Parse(dt.Rows[0]["cli_dt_nascimento"].ToString());
                cli.Cli_cpf = dt.Rows[0]["cli_cpf"].ToString();
                cli.Cli_rg = dt.Rows[0]["cli_rg"].ToString();
                cli.Cli_email = dt.Rows[0]["cli_email"].ToString();
                cli.Cli_observacao = dt.Rows[0]["cli_observacao"].ToString();
            }
            return cli;
        }

        public bool Gravar(Cliente c, out string msg)
        {
            msg = "";
            if (c.Cli_nome.Length < 5)
            {
                msg = "Nome muito pequeno.";
                return false;
            }
            if (c.Cli_endereco == "")
            {
                msg = "Informe o Endereço do Cliente.";
                return false;
            }
            if (c.Cidade.Cid_codigo == 0)
            {
                msg = "Informe a Cidade do Cliente.";
                return false;
            }
            if (c.Cli_dt_nascimento.ToString()=="")
            {
                msg = "Informe a Data de Nascimento do Cliente.";
                return false;
            }
            if (c.Cli_telefone == "")
            {
                msg = "Informe o Telefone do Cliente.";
                return false;
            }
            if (c.Cli_email == "")
            {
                msg = "Informe o E-mail do Cliente.";
                return false;
            }

            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "";
            Dictionary<string, object> ps = new Dictionary<string, object>();
         
            if (c.Cli_codigo == 0)
            {
                sql = @"insert into cliente (cli_nome,cli_endereco,cli_numero,cli_bairro,cid_codigo,cli_cep,
                                             cli_telefone,cli_celular,cli_dt_nascimento,cli_cpf,cli_rg,cli_email,cli_observacao) 
                                     values (@cli_nome,@cli_endereco,@cli_numero,@cli_bairro,@cid_codigo,@cli_cep,
                                             @cli_telefone,@cli_celular,@cli_dt_nascimento,@cli_cpf,@cli_rg,@cli_email,@cli_observacao)";
            }
            else
            {
                sql = @"update cliente 
                            set cli_nome = @cli_nome, 
                                cli_endereco = @cli_endereco,
                                cli_numero = @cli_numero,
                                cli_bairro = @cli_bairro,
                                cid_codigo = @cid_codigo,
                                cli_cep = @cli_cep,
                                cli_telefone = @cli_telefone,
                                cli_celular = @cli_celular,
                                cli_dt_nascimento = @cli_dt_nascimento,
                                cli_cpf = @cli_cpf,
                                cli_rg = @cli_rg,
                                cli_email = @cli_email,
                                cli_observacao = @cli_observacao
                         where cli_codigo = @cli_codigo";
                ps.Add("@cli_codigo", c.Cli_codigo);
            }
            ps.Add("@cli_nome", c.Cli_nome);
            ps.Add("@cli_endereco", c.Cli_endereco);
            ps.Add("@cli_numero", c.Cli_numero);
            ps.Add("@cli_bairro", c.Cli_bairro);
            ps.Add("@cid_codigo", c.Cidade.Cid_codigo);
            ps.Add("@cli_cep", c.Cli_cep);
            ps.Add("@cli_telefone", c.Cli_telefone);
            ps.Add("@cli_celular", c.Cli_celular);
            ps.Add("@cli_dt_nascimento", c.Cli_dt_nascimento);
            ps.Add("@cli_cpf", c.Cli_cpf);
            ps.Add("@cli_rg", c.Cli_rg);
            ps.Add("@cli_email", c.Cli_email);
            ps.Add("@cli_observacao", c.Cli_observacao);
            int r = bd.ExecutarComando(sql, ps);
            msg = bd.MsgErro;
            return r == 1;
        }

        public bool Excluir(int cli_codigo)
        {
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "delete from cliente where cli_codigo = @cli_codigo";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cli_codigo", cli_codigo);
            int linhasAfetadas = bd.ExecutarComando(sql, ps);
            return linhasAfetadas > 0;
        }

    }
}
