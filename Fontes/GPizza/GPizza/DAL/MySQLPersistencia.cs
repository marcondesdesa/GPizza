using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GPizza.DAL
{
    public class MySQLPersistencia
    {
        MySqlConnection _conexao;
        MySqlCommand _cmd;
        MySqlTransaction _transacao;

        long _ultimoId = 0;
        string _msgErro = "";
        string _msgErroTecnica = "";
        bool _comTransacao = false;

        public long UltimoId { get => _ultimoId; }
        public string MsgErro { get => _msgErro; }
        public string MsgErroTecnica { get => _msgErroTecnica; }
       // public bool ComTransacao { get => _comTransacao; set => _comTransacao = value; }

        public MySQLPersistencia()
        {
            string strcon = "Server=den1.mysql3.gear.host;Database=gpizza;Uid=gpizza;Pwd=a123456$;";
            _conexao = new MySqlConnection(strcon);
            _cmd = _conexao.CreateCommand();

        }
        /// <summary>
        /// Abre a conexão...
        /// </summary>
        public void Abrir()
        {
            if (_conexao.State != System.Data.ConnectionState.Open)
                _conexao.Open();
        }

        /// <summary>
        /// Fechar a conexão.
        /// </summary>
        public void Fechar()
        {
            _conexao.Close();
        }

        public void IniciarTransacao() {
            _comTransacao = true;
            Abrir();
            _transacao = _conexao.BeginTransaction();
            _cmd.Transaction = _transacao;
        }

        public void CommitarTransacao() {

            if (_transacao != null)
            {
                _transacao.Commit();
                _transacao = null;
                _comTransacao = false;
                Fechar();
            }
        }

        public void CancelarTransacao()
        {
            if (_transacao != null)
            {
                _transacao.Rollback();
                _transacao = null;
                _comTransacao = false;
                Fechar();

            }
        }

        /// <summary>
        /// Usado para executar comandos Insert, Delete e Update, além de executar Stored Procedure.
        /// </summary>
        /// <param name="comando">Comando a ser executado</param>
        public int ExecutarComando(string comando)
        {

            int linhaAfetadas = 0;
            _cmd.CommandText = comando;

            Abrir();

            try
            {
                //tenta
                linhaAfetadas = _cmd.ExecuteNonQuery();
                _ultimoId = _cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                CancelarTransacao();
                //erro
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível executar.";
                linhaAfetadas = -1;
            }
            finally
            {
                //sempre passará por aqui (no erro ou no acerto)
                if (!_comTransacao)
                {
                    Fechar();
                }
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
            _cmd.CommandText = comando;

            foreach (var item in parametros)
            {
                _cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            Abrir();

            try
            {
                //tenta
                linhaAfetadas = _cmd.ExecuteNonQuery();
                _ultimoId = _cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                //erro
                CancelarTransacao();
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível executar.";
                linhaAfetadas = -1;
            }
            finally
            {
                //sempre passará por aqui (no erro ou no acerto)
                if (!_comTransacao)
                {
                    Fechar();
                }

             }

            return linhaAfetadas;

        }

        public DataTable ExecutarConsulta(string comando)
        {
            DataTable dt = new DataTable();

            _cmd.CommandText = comando;

            Abrir();
            try
            {
                dt.Load(_cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                CancelarTransacao();
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível obter os dados.";
            }
            finally
            {
                if (!_comTransacao)
                {
                    Fechar();
                }
            }

            return dt;
        }

        public DataTable ExecutarConsulta(string comando, Dictionary<string, object> parametros)
        {
            DataTable dt = new DataTable();

            _cmd.CommandText = comando;
            foreach (var item in parametros)
            {
                _cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            Abrir();
            try
            {
                dt.Load(_cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                CancelarTransacao();
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível obter os dados.";
            }
            finally
            {
                if (!_comTransacao)
                {
                    Fechar();
                }
            }

            return dt;
        }

        public object ExecutarAgregacao(string comando)
        {
            object valor = null;

            _cmd.CommandText = comando;

            Abrir();
            try
            {
                valor = _cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                CancelarTransacao();
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível obter os dados.";
            }
            finally
            {
                if (!_comTransacao)
                {
                    Fechar();
                }
            }

            return valor;
        }

        public object ExecutarAgregacao(string comando, Dictionary<string, object> parametros)
        {
            object valor = null;

            _cmd.CommandText = comando;
            foreach (var item in parametros)
            {
                _cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            Abrir();
            try
            {
                valor = _cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                CancelarTransacao();
                _msgErroTecnica = ex.Message;
                _msgErro = "Não foi possível obter os dados.";
            }
            finally
            {
                if (!_comTransacao)
                {
                    Fechar();
                }
            }

            return valor;
        }

        public void LimparParametros()
        {
            _cmd.Parameters.Clear();
        }
    }
}
