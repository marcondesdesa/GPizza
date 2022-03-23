using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static GPizza.Funcoes;

namespace GPizza.Models
{
    public class Pedido
    {
        private int _ped_codigo;
        private DateTime _ped_data;
        private Funcionario _funcionario;
        private Cliente _cliente;      
        private double _ped_desconto;
        private double _ped_valor_total;
        private string _ped_observacao;
        private List<PedidoItem> _itens;

        public Pedido()
        {
            Ped_codigo = 0;
            Ped_data = DateTime.Now;
            Funcionario = null;
            Cliente = null;
            Ped_desconto = 0;
            Ped_valor_total = 0;
            this.Ped_observacao = "";
            Itens = new List<PedidoItem>();
        }

        public Pedido(int ped_codigo, DateTime ped_data, Funcionario funcionario, Cliente cliente, double ped_desconto, double ped_valor_total, string ped_observacao, List<PedidoItem> itens)
        {
            Ped_codigo = ped_codigo;
            Ped_data = ped_data;
            Funcionario = funcionario;
            Cliente = cliente;
            Ped_desconto = ped_desconto;
            Ped_valor_total = ped_valor_total;
            this.Ped_observacao = ped_observacao;
            Itens = itens;
        }

        public int Ped_codigo { get => _ped_codigo; set => _ped_codigo = value; }
        public DateTime Ped_data { get => _ped_data; set => _ped_data = value; }
        public Funcionario Funcionario { get => _funcionario; set => _funcionario = value; }
        public Cliente Cliente { get => _cliente; set => _cliente = value; }
        public double Ped_desconto { get => _ped_desconto; set => _ped_desconto = value; }
        public double Ped_valor_total { get => _ped_valor_total; set => _ped_valor_total = value; }
        public string Ped_observacao { get => _ped_observacao; set => _ped_observacao = value; }
        public List<PedidoItem> Itens { get => _itens; set => _itens = value; }

        public bool Gravar(out string msg)
        {            
            if (this.Cliente.Cli_codigo == 0)
            {
                msg = "Informe o Cliente.";
                return false;
            }
                         
            bool retorno = false;
            string erro = "";
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            Dictionary<string, object> pSQL1 = new Dictionary<string, object>();
            try
            {
                bd.IniciarTransacao();
                string sql1 = "";
                if (this.Ped_codigo == 0) //Novo Pedido
                {
                    sql1 = "insert into pedido (ped_data, fun_codigo, cli_codigo, ped_desconto, ped_valor_total, ped_observacao) " +
                        "                       values(@ped_data,@fun_codigo,@cli_codigo,@ped_desconto,@ped_valor_total,@ped_observacao) ";

                }
                else
                {
                    sql1 = @"update pedido 
                            set ped_data = @ped_data, 
                                fun_codigo = @fun_codigo,
                                cli_codigo = @cli_codigo,
                                ped_desconto = @ped_desconto,
                                ped_valor_total = @ped_valor_total,
                                ped_observacao = @ped_observacao
                           where ped_codigo = @ped_codigo";
                    pSQL1.Add("@ped_codigo", this.Ped_codigo);
                }              
                pSQL1.Add("@ped_data", Ped_data);
                pSQL1.Add("@fun_codigo", Funcionario.Fun_codigo);
                pSQL1.Add("@cli_codigo", Cliente.Cli_codigo);
                pSQL1.Add("@ped_desconto", Ped_desconto);
                pSQL1.Add("@ped_valor_total", Ped_valor_total);
                pSQL1.Add("@ped_observacao", Ped_observacao);
                bd.ExecutarComando(sql1, pSQL1);
                int ped_codigo = (int)bd.UltimoId;

                if (ped_codigo > 0)
                {
                    foreach (PedidoItem item in Itens)
                    {
                        item.Gravar(ped_codigo, out erro, bd);
                        if (erro != "") 
                            break;
                    }
                    if (erro == "")
                    {
                        //Erro ao Gravar o Pedido (Tabela PEDIDO_ITEM)
                        bd.CommitarTransacao();
                        retorno = true;
                    }
                    else
                    {
                        bd.CancelarTransacao();
                        retorno = false;
                    }
                }
                else
                {
                    //Erro ao Gravar o Pedido (Tabela PEDIDO)
                    erro = bd.MsgErroTecnica;
                    bd.CancelarTransacao();
                    retorno = false;
                }
            }
            catch (Exception ex)
            {
                erro = "[GravaPedido]: " + ex.Message;
                bd.CancelarTransacao();
                retorno = false;
            }
            msg = erro;
            return retorno;
        }

        public Pedido getPedido(int ped_codigo)
        {
            Pedido ped = null;
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = " select * from pedido p where (p.ped_codigo = @ped_codigo)";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@ped_codigo", ped_codigo);
            DataTable dtPed = bd.ExecutarConsulta(sql, ps);

            if (dtPed.Rows.Count > 0)
            {
                ped = new Pedido();           
                ped.Ped_codigo = getInt(dtPed.Rows[0]["ped_codigo"].ToString());
                ped.Ped_data = getDateTime(dtPed.Rows[0]["ped_data"].ToString());

                Cliente c = new Cliente().getCliente(getInt(dtPed.Rows[0]["cli_codigo"].ToString()));
                ped.Cliente = c;

                Funcionario f = new Funcionario().getFuncionario(getInt(dtPed.Rows[0]["cli_codigo"].ToString()));
                ped.Funcionario = f;

                List<PedidoItem> itens = new PedidoItem().Pesquisar(getInt(dtPed.Rows[0]["ped_codigo"].ToString()));
                ped.Itens = itens;

                ped.Ped_desconto = getDouble(dtPed.Rows[0]["ped_desconto"].ToString());
                ped.Ped_valor_total = getDouble(dtPed.Rows[0]["ped_valor_total"].ToString());
                ped.Ped_observacao = dtPed.Rows[0]["ped_observacao"].ToString();                           
            }
            return ped;
        }

        public List<Pedido> Pesquisar(string cli_nome)
        {
            List<Pedido> pedidos = new List<Pedido>();
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = " select p.ped_codigo,p.cli_codigo,c.cli_codigo,c.cli_nome "+
                         " from pedido p                                            "+
                         " inner join cliente c on (p.cli_codigo=c.cli_codigo)      "+
                         " where (c.cli_nome like @cli_nome) order by c.cli_nome    ";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@cli_nome", cli_nome + "%");
            DataTable dt = bd.ExecutarConsulta(sql, ps);
            foreach (DataRow linha in dt.Rows)
            {
                Pedido p = new Pedido().getPedido(Convert.ToInt32(linha["ped_codigo"]));
                pedidos.Add(p);
            }
            return pedidos;
        }

        public void Obter(int ped_codigo)
        {
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = " select * from pedido p where (p.ped_codigo = @ped_codigo)";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@ped_codigo", ped_codigo);
            DataTable dtPed = bd.ExecutarConsulta(sql, ps);

            if (dtPed.Rows.Count > 0)
            {
                this.Ped_codigo = getInt(dtPed.Rows[0]["ped_codigo"].ToString());
                this.Ped_data = getDateTime(dtPed.Rows[0]["ped_data"].ToString());

                Cliente c = new Cliente().getCliente(getInt(dtPed.Rows[0]["cli_codigo"].ToString()));
                this.Cliente = c;

                Funcionario f = new Funcionario().getFuncionario(getInt(dtPed.Rows[0]["fun_codigo"].ToString()));
                this.Funcionario = f;

                List<PedidoItem> itens = new PedidoItem().Pesquisar(getInt(dtPed.Rows[0]["ped_codigo"].ToString()));
                this.Itens = itens;

                this.Ped_desconto = getDouble(dtPed.Rows[0]["ped_desconto"].ToString());
                this.Ped_valor_total = getDouble(dtPed.Rows[0]["ped_valor_total"].ToString());
                this.Ped_observacao = dtPed.Rows[0]["ped_observacao"].ToString();
            }
        }

    }
}
