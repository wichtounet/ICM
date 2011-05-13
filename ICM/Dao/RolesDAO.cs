using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using ICM.Model;
using ICM.Utils;
using NLog;

namespace ICM.Dao
{
    /// <summary>
    ///  This class enables the user to search the database for roles. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public class RolesDAO
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns all the roles of the database. 
        /// </summary>
        /// <returns>a List containing all the roles</returns>
        public List<Role> GetAllRoles(SqlConnection connection)
        {
            Logger.Debug("Get all roles");

            var roles = new List<Role>();

            using (var reader = DBUtils.ExecuteQuery("Select name from [Role]", connection, IsolationLevel.ReadUncommitted, new NameValueCollection()))
            {
                while (reader.Read())
                {
                    roles.Add(BindRole(reader));
                }
            }

            Logger.Debug("Found {0} roles", roles.Count);

            return roles;
        }

        /// <summary>
        /// Bind the SQL Result to a Role object
        /// </summary>
        /// <param name="result">The result of a SQL query</param>
        /// <returns>a new instance of Role with the values of the SQL Result</returns>
        private static Role BindRole(SqlResult result)
        {
            return new Role
            {
                Name = result["name"].ToString()
            };
        }
    }
}