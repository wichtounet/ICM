using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

namespace ICM.Utils
{
    /// <summary>
    ///  This utility class contains several methods that aims to simplify the use of database. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public class DBUtils
    {
        public static SqlTransaction BeginTransaction(IsolationLevel level)
        {
            var connection = DBManager.GetInstance().GetConnection();
            var transaction = connection.BeginTransaction(level);
            return transaction;
        }

        public static void CommitTransaction(SqlTransaction transaction)
        {
            transaction.Commit();
        }

        public static SqlResult ExecuteTransactionQuery(string sql, SqlTransaction transaction, NameValueCollection parameters)
        {
            var connection = DBManager.GetInstance().GetConnection();

            var command = new SqlCommand(sql, connection, transaction);

            foreach (var key in parameters.AllKeys)
            {
                command.Parameters.AddWithValue(key, parameters.Get(key));
            }

            var reader = command.ExecuteReader();

            return new SqlResult(transaction, reader);
        }

        public static SqlResult ExecuteTransactionQuery(string sql, SqlTransaction transaction)
        {
            return ExecuteTransactionQuery(sql, transaction, new NameValueCollection());
        }

        public static void ExecuteUpdate(string sql, IsolationLevel level, NameValueCollection parameters)
        {
            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(level);

            ExecuteNonQuery(connection, sql, transaction, parameters);

            transaction.Commit();
        }

        public static int ExecuteInsert(string sql, IsolationLevel level, NameValueCollection parameters, string tableName)
        {
            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(level);

            ExecuteNonQuery(connection, sql, transaction, parameters);

            var getCommand = new SqlCommand("SELECT IDENT_CURRENT(@table) AS ID", connection, transaction);
            getCommand.Parameters.AddWithValue("table", tableName);

            int id;
            using (var reader = getCommand.ExecuteReader())
            {
                reader.Read();

                id = reader["ID"].ToString().ToInt();
            }

            transaction.Commit();

            return id;
        }

        public static void ExecuteNonQuery(SqlConnection connection, string sql, SqlTransaction transaction, NameValueCollection parameters)
        {
            var command = new SqlCommand(sql, connection, transaction);

            foreach (var key in parameters.AllKeys)
            {
                command.Parameters.AddWithValue(key, parameters.Get(key));
            }

            command.ExecuteNonQuery();
        }

        public static SqlResult ExecuteQuery(string sql, IsolationLevel level, NameValueCollection parameters)
        {
            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(level);

            var command = new SqlCommand(sql, connection, transaction);

            foreach (var key in parameters.AllKeys)
            {
                command.Parameters.AddWithValue(key, parameters.Get(key));
            }

            var reader = command.ExecuteReader();

            return new SqlResult(transaction, reader);
        }

        public static SqlResult ExecuteQuery(string sql, IsolationLevel level)
        {
            return ExecuteQuery(sql, level, new NameValueCollection());
        }
    }

    public class SqlResult : IDisposable
    {
        private readonly SqlDataReader reader;
        private readonly SqlTransaction transaction;

        public SqlResult(SqlTransaction transaction, SqlDataReader reader)
        {
            this.transaction = transaction;
            this.reader = reader;
        }

        public bool Read()
        {
            return reader.Read();
        }

        public object this[string name]
        {
            get { return reader[name]; }
        }

        public void Dispose()
        {
            reader.Close();
            transaction.Commit();
        }
    }
}