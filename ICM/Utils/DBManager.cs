using System.Data.SqlClient;

namespace ICM.Utils
{
    public class DBManager
    {
        private static readonly DBManager Instance = new DBManager();

        private SqlConnection connection;

        private DBManager()
        {
            //Nothing to do more
        }

        public SqlConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new SqlConnection(@"Data Source=160.98.60.35\MSSQLSERVER,1433;Initial Catalog=ICM;Integrated Security=False;User ID=sa;Password=International3");
                
                connection.Open();
            }

            return connection;
        }

        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        public static DBManager GetInstance()
        {
            return Instance;
        }
    }
}