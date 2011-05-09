using System;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace ICM.Utils
{
    ///<summary>
    /// A little utility class to open connections to the database and manage them. This class is a singleton, you cannot instantiate it, you must use GetInstance to use it. 
    ///</summary>
    public class DBManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly DBManager Instance = new DBManager();

        private SqlConnection connection;

        private DBManager()
        {
            Logger.Debug("Create DBManager");
        }

        public SqlConnection GetConnection()
        {
            if (connection == null || connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection = GetNewConnection();
            }

            return connection;
        }

        ///<summary>
        /// Create a new connection to the database. 
        ///</summary>
        ///<returns>A new connection to the database, already in open state</returns>
        public SqlConnection GetNewConnection()
        {
            Logger.Debug("Open connection");

            var newConnection = new SqlConnection(@"Data Source=160.98.60.35\MSSQLSERVER,1433;Initial Catalog=ICM;Integrated Security=False;User ID=sa;Password=International3;");

            newConnection.Open();

            return newConnection;
        }

        public void CloseConnection(SqlConnection connection)
        {
            Logger.Debug("Close connection");

            connection.Close();
        }

        public void Close()
        {
            if (connection != null)
            {
                Logger.Debug("Close connection");

                connection.Close();

                connection = null;
            }
        }

        ///<summary>
        /// Returns the unique instance of the class. 
        ///</summary>
        ///<returns>The singleton instance of the class. </returns>
        public static DBManager GetInstance()
        {
            return Instance;
        }
    }
}