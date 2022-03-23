using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GPizza.Models
{
    public class PedidoItem
    {
        private Produto produto;
        private int pi_quant;
        private double pi_valor_unit;
        private double pi_valor_tot;

        public PedidoItem()
        {
            this.Produto = null;
            this.Pi_quant = 0;
            this.Pi_valor_unit = 0;
            this.Pi_valor_tot = 0;
        }

        public PedidoItem(Produto produto, int pi_quant, double pi_valor_unit, double pi_valor_tot)
        {
            this.Produto = produto;
            this.Pi_quant = pi_quant;
            this.Pi_valor_unit = pi_valor_unit;
            this.Pi_valor_tot = pi_valor_tot;
        }

        public Produto Produto { get => produto; set => produto = value; }
        public int Pi_quant { get => pi_quant; set => pi_quant = value; }
        public double Pi_valor_unit { get => pi_valor_unit; set => pi_valor_unit = value; }
        public double Pi_valor_tot { get => pi_valor_tot; set => pi_valor_tot = value; }

        public PedidoItem getPedidoItem(int ped_codigo)
        {
            PedidoItem pi = null;
            DAL.MySQLPersistencia bd = new DAL.MySQLPersistencia();
            string sql = "select * from pedido_item where (ped_codigo = @ped_codigo and pro_codigo = @pro_codigo)";

            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@ped_codigo", ped_codigo);
            ps.Add("@pro_codigo", this.Produto.Pro_codigo);
            DataTable dt = bd.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            if (dt.Rows.Count > 0)
            {
                pi = new PedidoItem();
                pi.Produto = new Produto().getProduto(Convert.ToInt32(dt.Rows[0]["pro_codigo"]));
                pi.Pi_quant = Convert.ToInt32(dt.Rows[0]["pi_quant"]);
                pi.Pi_valor_unit = Convert.ToDouble(dt.Rows[0]["pi_valor_unit"].ToString());
                pi.Pi_valor_tot = Convert.ToDouble(dt.Rows[0]["pi_valor_tot"].ToString());
            }
            return pi;
        }

        public bool Gravar(int ped_codigo, out string msg, DAL.MySQLPersistencia conexao)
        {
            string erro = "";
            bool retorno = false;
            if (ped_codigo > 0)
            {
                try
                {
                    string sqlItem = "";
                    Dictionary<string, object> psqlItem = new Dictionary<string, object>();
                    PedidoItem pedidoitem = getPedidoItem(ped_codigo); //Verifica se ja existe o Item add

                    if (pedidoitem == null) //Não existe esse item ja lancado
                    {
                        sqlItem = "insert into pedido_item (ped_codigo, pro_codigo, pi_quant, pi_valor_unit, pi_valor_tot) " +
                                    "                 values (@ped_codigo,@pro_codigo, @pi_quant, @pi_valor_unit, @pi_valor_tot) ";
                        psqlItem.Add("@ped_codigo", ped_codigo);
                        psqlItem.Add("@pro_codigo", Produto.Pro_codigo);
                    }
                    else
                    {
                        sqlItem = @"update pedido_item 
                        set pi_quant = @pi_quant, 
                            pi_valor_unit = @pi_valor_unit,
                            pi_valor_tot = @pi_valor_tot
                        where (ped_codigo = @ped_codigo and pro_codigo = @pro_codigo) ";
                        psqlItem.Add("@ped_codigo", ped_codigo);
                        psqlItem.Add("@pro_codigo", this.Produto.Pro_codigo);
                    }
                    psqlItem.Add("@pi_quant", Pi_quant);
                    psqlItem.Add("@pi_valor_unit", Pi_valor_unit);
                    psqlItem.Add("@pi_valor_tot", Pi_valor_tot);
                    conexao.LimparParametros();
                    conexao.ExecutarComando(sqlItem, psqlItem);
                    retorno = true;

                }
                catch (Exception ex)
                {
                    erro = "[GravaItem]: " + ex.Message;
                    retorno = false;
                }
            }
            msg = erro;
            return retorno;
        }

        public List<PedidoItem> Pesquisar(int ped_codigo)
        {
            List<PedidoItem> pedidos = new List<PedidoItem>();
            DAL.MySQLPersistencia bdItem = new DAL.MySQLPersistencia();
            string sql = "select * from pedido_item where (ped_codigo = @ped_codigo)";
            Dictionary<string, object> ps = new Dictionary<string, object>();
            ps.Add("@ped_codigo", ped_codigo);
            DataTable dtItem = bdItem.ExecutarConsulta(sql, ps);
            //Mapeando linhas de tabela em objetos.
            foreach (DataRow linha in dtItem.Rows)
            {
                PedidoItem pi = new PedidoItem();
                pi.Produto = new Produto().getProduto(Convert.ToInt32(dtItem.Rows[0]["pro_codigo"]));
                pi.Pi_quant = Convert.ToInt32(dtItem.Rows[0]["pi_quant"]);
                pi.Pi_valor_unit = Convert.ToDouble(dtItem.Rows[0]["pi_valor_unit"].ToString());
                pi.Pi_valor_tot = Convert.ToDouble(dtItem.Rows[0]["pi_valor_tot"].ToString());
                pedidos.Add(pi);
            }
            return pedidos;
        }
    }
}
