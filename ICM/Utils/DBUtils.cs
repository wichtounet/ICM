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

        public static SqlResult ExecuteTransactionQuery(string sql, SqlTransaction transaction, NameValueCollection parameters)
        {
            var command = new SqlCommand(sql, transaction.Connection, transaction);

            foreach (var key in parameters.AllKeys)
            {
                command.Parameters.AddWithValue(key, parameters.Get(key));
            }

            var reader = command.ExecuteReader();

            return new SqlResult(null, reader);
        }

        public static SqlResult ExecuteTransactionQuery(string sql, SqlTransaction transaction)
        {
            return ExecuteTransactionQuery(sql, transaction, new NameValueCollection());
        }

        public static int ExecuteTransactionInsert(string sql, SqlTransaction transaction, NameValueCollection parameters, string tableName)
        {
            ExecuteNonQuery(sql, transaction, parameters);

            var getCommand = new SqlCommand("SELECT IDENT_CURRENT(@table) AS ID", transaction.Connection, transaction);
            getCommand.Parameters.AddWithValue("table", tableName);

            int id;
            using (var reader = getCommand.ExecuteReader())
            {
                reader.Read();
                id = reader["ID"].ToString().ToInt();
            }

            return id;
        }

        ///<summary>
        /// Execute an update on the database. 
        ///</summary>
        ///<param name="sql">The SQL Query</param>
        ///<param name="level">The isolation level</param>
        ///<param name="parameters">The SQL Parameters</param>
        /// <remarks>
        /// This method open a new connection and close it
        /// </remarks>
        public static void ExecuteUpdate(string sql, IsolationLevel level, NameValueCollection parameters)
        {
            using (var connection = DBManager.GetInstance().GetNewConnection())
            {
                var transaction = connection.BeginTransaction(level);

                ExecuteNonQuery(sql, transaction, parameters);

                transaction.Commit();
            }
        }
        
        public static int ExecuteInsert(string sql, IsolationLevel level, NameValueCollection parameters, string tableName, SqlTransaction transaction)
        {
            ExecuteNonQuery(sql, transaction, parameters);

            var getCommand = new SqlCommand("SELECT IDENT_CURRENT(@table) AS ID", transaction.Connection, transaction);
            getCommand.Parameters.AddWithValue("table", tableName);

            int id;
            using (var reader = getCommand.ExecuteReader())
            {
                reader.Read();

                id = reader["ID"].ToString().ToInt();
            }

            return id;
        }

        public static void ExecuteNonQuery(string sql, SqlTransaction transaction, NameValueCollection parameters)
        {
            var command = new SqlCommand(sql, transaction.Connection, transaction);

            foreach (var key in parameters.AllKeys)
            {
                command.Parameters.AddWithValue(key, parameters.Get(key));
            }

            command.ExecuteNonQuery();
        }

        public static SqlResult ExecuteQuery(string sql, SqlConnection connection, IsolationLevel level, NameValueCollection parameters)
        {
            var transaction = connection.BeginTransaction(level);

            var command = new SqlCommand(sql, connection, transaction);

            foreach (var key in parameters.AllKeys)
            {
                command.Parameters.AddWithValue(key, parameters.Get(key));
            }

            var reader = command.ExecuteReader();

            return new SqlResult(transaction, reader);
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

    ///<summary>
    /// A simple container for a data reader. 
    ///</summary>
    public class SqlResult : IDisposable
    {
        private readonly SqlDataReader reader;
        private readonly SqlTransaction transaction;

        ///<summary>
        /// Create a new SqlResult with the given transaction and reader. If transaction is not null, it will be closed on dispose. 
        ///</summary>
        ///<param name="transaction">The transaction</param>
        ///<param name="reader">The reader to use</param>
        public SqlResult(SqlTransaction transaction, SqlDataReader reader)
        {
            this.transaction = transaction;
            this.reader = reader;
        }

        ///<summary>
        /// Read on the reader. 
        ///</summary>
        ///<returns>true if the reader has more data otherwise false</returns>
        public bool Read()
        {
            return reader.Read();
        }

        ///<summary>
        /// Return the object with the given name. 
        ///</summary>
        ///<param name="name">The name of the object in the reader</param>
        public object this[string name]
        {
            get { return reader[name]; }
        }

        public void Dispose()
        {
            reader.Close();

            if(transaction != null)
            {
                transaction.Commit();
            }
        }
    }
}