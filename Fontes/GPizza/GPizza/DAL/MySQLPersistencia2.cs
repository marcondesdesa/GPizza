using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GPizza.DAL
{
    public class MySQLPersistencia2
    {
        //private static MySQLPersistencia instancia;
        private MySqlConnection _conexao;
        MySqlCommand _cmd;
        long _ultimoId = 0;
        string _msgErro = "";
        string _msgErroTecnica = "";

        public long UltimoId { get => _ultimoId; }
        public string MsgErro { get => _msgErro; }
        public string MsgErroTecnica { get => _msgErroTecnica; }
        public MySqlConnection Conexao { get => _conexao; set => _conexao = value; }
        public MySqlCommand Cmd { get => _cmd; set => _cmd = value; }
        public long UltimoId1 { get => _ultimoId; set => _ultimoId = value; }
        public string MsgErro1 { get => _msgErro; set => _msgErro = value; }
        public string MsgErroTecnica1 { get => _msgErroTecnica; set => _msgErroTecnica = value; }

        public MySQLPersistencia2()
        {
            string strcon = "Server=den1.mysql3.gear.host;Database=gpizza;Uid=gpizza;Pwd=a123456$;";
            Conexao = new MySqlConnection(strcon);
            Cmd = Conexao.CreateCommand();
        }

        //public static MySQLPersistencia ConectaBanco()
        //{
        //    if (instancia == null)
        //    {
        //        instancia = new MySQLPersistencia();
        //    }
        //    //instancia = new MySQLPersistencia();
        //    return instancia;
        //}


        /// <summary>
        /// Abre a conexão...
        /// </summary>
        public void Abrir()
        {
            if (Conexao.State != System.Data.ConnectionState.Open)
                Conexao.Open();
        }

        /// <summary>
        /// Fechar a conexão.
        /// </summary>
        public void Fechar()
        {
            Conexao.Close();
            Cmd.Parameters.Clear();
        }


        /// <summary>
        /// Usado para executar comandos Insert, Delete e Update, além de executar Stored Procedure.
        /// </summary>
        /// <param name="comando">Comando a ser executado</param>
        public int ExecutarComando(string comando)
        {

            int linhaAfetadas = 0;
            Cmd.CommandText = comando;

            Abrir();

            try
            {
                //tenta
                linhaAfetadas = Cmd.ExecuteNonQuery();
                _ultimoId = Cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                //erro
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível executar.";
                linhaAfetadas = -1;
            }
            finally
            {
                //sempre passará por aqui (no erro ou no acerto)
                Fechar();
            }

            return linhaAfetadas;

        }


        /// <summary>
        /// Usado para executar comandos Insert, Delete e Update, além de executar Stored Procedure.
        /// </summary>
        /// <param name="comando">Comando a ser executado</param>
        /// <param name="parametros">Lista e parâmetros para o comando.</param>
        public int ExecutarComando(string comando,
            Dictionary<string, object> parametros)
        {

            int linhaAfetadas = 0;
            Cmd.CommandText = comando;

            foreach (var item in parametros)
            {
                Cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            Abrir();

            try
            {
                //tenta
                linhaAfetadas = Cmd.ExecuteNonQuery();
                _ultimoId = Cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                //erro
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível executar.";
                linhaAfetadas = -1;
            }
            finally
            {
                //sempre passará por aqui (no erro ou no acerto)
                Fechar();
            }

            return linhaAfetadas;

        }

        public DataTable ExecutarConsulta(string comando)
        {
            DataTable dt = new DataTable();

            Cmd.CommandText = comando;

            Abrir();
            try
            {
                dt.Load(Cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível obter os dados.";
            }
            finally
            {
                Fechar();
            }

            return dt;
        }

        public DataTable ExecutarConsulta(string comando, Dictionary<string, object> parametros)
        {
            DataTable dt = new DataTable();
            Cmd.Parameters.Clear();
            Cmd.CommandText = comando;
            foreach (var item in parametros)
            {
                Cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            Abrir();
            try
            {
                dt.Load(Cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível obter os dados.";
            }
            finally
            {
                Fechar();
            }

            return dt;
        }

        public object ExecutarAgregacao(string comando)
        {
            object valor = null;

            Cmd.CommandText = comando;

            Abrir();
            try
            {
                valor = Cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível obter os dados.";
            }
            finally
            {
                Fechar();
            }

            return valor;
        }

        public object ExecutarAgregacao(string comando, Dictionary<string, object> parametros)
        {
            object valor = null;

            Cmd.CommandText = comando;
            foreach (var item in parametros)
            {
                Cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            Abrir();
            try
            {
                valor = _cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível obter os dados.";
            }
            finally
            {
                Fechar();
            }

            return valor;
        }

    }
}
