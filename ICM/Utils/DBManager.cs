using System;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace ICM.Utils
{
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
                Logger.Debug("Open connection");

                connection = new SqlConnection(@"Data Source=160.98.60.35\MSSQLSERVER,1433;Initial Catalog=ICM;Integrated Security=False;User ID=sa;Password=International3");
                
                connection.Open();
            }

            return connection;
        }

        public SqlConnection GetNewConnection()
        {
            Logger.Debug("Open connection");

            var newConnection = new SqlConnection(@"Data Source=160.98.60.35\MSSQLSERVER,1433;Initial Catalog=ICM;Integrated Security=False;User ID=sa;Password=International3;Connect Timeout=5");

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

        public static DBManager GetInstance()
        {
            return Instance;
        }
    }
}