using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using ICM.Model;
using ICM.Dao;
using ICM.Utils;
using NLog;

namespace ICM.Dao
{
    /// <summary>
    /// Gives access to the users account data stored in the database.
    /// </summary>
    public class UsersDAO
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates and fills a list of users representing all the users accounts stored in the database.
        /// </summary>
        /// <returns>The users list.</returns>
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            SqlResult reader;

            Logger.Debug("Retrieving all the users.");

            using (reader = DBUtils.ExecuteQuery("SELECT * FROM [User]", System.Data.IsolationLevel.ReadUncommitted))
            {
                while (reader.Read())
                {
                    User user = new User();
                    user.Login = (string)reader["login"];
                    user.Admin = (bool)reader["admin"];
                    user.Password = (string)reader["password"];
                    users.Add(user);
                }
            }

            Logger.Debug("Retrieved all the users.");

            return users;
        }
    }
}