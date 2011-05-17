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

        private DBManager()
        {
            Logger.Debug("Create DBManager");
        }

        ///<summary>
        /// Create a new connection to the database. 
        ///</summary>
        ///<returns>A new connection to the database, already in open state</returns>
        public SqlConnection GetNewConnection()
        {
            Logger.Debug("Open connection");

            var newConnection = new SqlConnection(@"Data Source=160.98.60.35\MSSQLSERVER,1433;Initial Catalog=ICM;Integrated Security=False;User ID=icm_user;Password=International3;");

            newConnection.Open();

            return newConnection;
        }

        ///<summary>
        /// Close the given connection. 
        ///</summary>
        ///<param name="connection">The connection to close</param>
        public void CloseConnection(SqlConnection connection)
        {
            Logger.Debug("Close connection");

            connection.Close();
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