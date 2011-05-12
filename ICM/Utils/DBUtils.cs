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
    public static class DBUtils
    {
        ///<summary>
        /// Execute a simple query in the given transaction and return the result of the query. 
        ///</summary>
        ///<param name="sql">The sql query to execute. </param>
        ///<param name="transaction">The transaction to use. </param>
        ///<param name="parameters">The parameters of the query. </param>
        ///<returns>The SQL result</returns>
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

        ///<summary>
        /// Execute a simple query in the given transaction and return the result of the query. 
        ///</summary>
        ///<param name="sql">The sql query to execute. </param>
        ///<param name="transaction">The transaction to use. </param>
        ///<returns>The SQL result</returns>
        public static SqlResult ExecuteTransactionQuery(string sql, SqlTransaction transaction)
        {
            return ExecuteTransactionQuery(sql, transaction, new NameValueCollection());
        }

        ///<summary>
        /// Execute a simple insert query in the given transaction and return the id of the inserted object. 
        ///</summary>
        ///<param name="sql">The sql query to execute. </param>
        ///<param name="transaction">The transaction to use. </param>
        ///<param name="parameters">The parameters of the query. </param>
        ///<param name="tableName">The name of the table</param>
        ///<returns>The id of the insert object</returns>
        public static int ExecuteInsert(string sql, NameValueCollection parameters, string tableName, SqlTransaction transaction)
        {
            ExecuteNonQuery(sql, transaction, parameters);

            var getCommand = new SqlCommand("SELECT IDENT_CURRENT(@table) AS ID", transaction.Connection, transaction);
            getCommand.Parameters.AddWithValue("@table", tableName);

            int id;
            using (var reader = getCommand.ExecuteReader())
            {
                reader.Read();

                id = Int32.Parse(reader["ID"].ToString());
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
        
        ///<summary>
        /// Execute an update query in the given transaction. 
        ///</summary>
        ///<param name="sql">The sql query to execute. </param>
        ///<param name="transaction">The transaction to use. </param>
        ///<param name="parameters">The parameters of the query. </param>
        public static void ExecuteNonQuery(string sql, SqlTransaction transaction, NameValueCollection parameters)
        {
            var command = new SqlCommand(sql, transaction.Connection, transaction);

            foreach (var key in parameters.AllKeys)
            {
                command.Parameters.AddWithValue(key, parameters.Get(key));
            }

            command.ExecuteNonQuery();
        }

        ///<summary>
        /// Execute a query in the given connection in a new transaction. 
        ///</summary>
        ///<param name="sql">The sql query to execute. </param>
        ///<param name="connection">The connection to use. </param>
        ///<param name="parameters">The parameters of the query. </param>
        ///<param name="level">The isolation level to use for the new transaction</param>
        ///<returns>The SQL Result</returns>
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