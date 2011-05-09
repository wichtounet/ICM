using System.Collections.Specialized;
using System.Data.SqlClient;
using ICM.Model;
using ICM.Utils;
using System.Data;
using System.Collections.Generic;
using NLog;

namespace ICM.Dao
{
    /// <summary>
    /// Gives access to the types stored in the database.
    /// </summary>
    /// <remarks>Kean Mariotti</remarks>
    public class TypesDAO
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates and fills a list containing all the types stored in the database.
        /// </summary>
        /// <returns>The list of types.</returns>

        public List<TypeContract> GetAllTypes(SqlConnection connection)

        /// <summary>
        /// Creates and fills a list containing all the types stored in the database.
        /// </summary>
        /// <returns>The list of types.</returns>
        {
            var types = new List<TypeContract>();

            Logger.Debug("Retrieving all the types.");

            using (var reader = DBUtils.ExecuteQuery("SELECT name FROM [typeContract]", connection, IsolationLevel.ReadUncommitted, new NameValueCollection()))
            {
                while (reader.Read())
                {
                    types.Add(BindType(reader));
                }
            }

            Logger.Debug("Retrieved all the types.");

            return types;
        }

        /// <summary>
        /// Utility function used to initialize a type with its data stored in database.
        /// </summary>
        /// <param name="reader">The SqlResult object used to read data from the database.</param>
        /// <returns>The initialized TypeContract object.</returns>
        private TypeContract BindType(SqlResult reader)
        {
            return new TypeContract {Name = reader["name"].ToString()};
        }
    }
}