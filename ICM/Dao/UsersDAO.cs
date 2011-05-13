using System.Collections.Generic;
using System.Collections.Specialized;
using ICM.Model;
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
            var users = new List<User>();

            Logger.Debug("Retrieving all the users.");

            using(var connection = DBManager.GetInstance().GetNewConnection())
            {
                using (var reader = DBUtils.ExecuteQuery("SELECT login, admin, password FROM [User]", connection, System.Data.IsolationLevel.ReadUncommitted, new NameValueCollection()))
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            Login = (string) reader["login"],
                            Admin = (bool) reader["admin"],
                            Password = (string) reader["password"]
                        };

                        users.Add(user);
                    }
                }
            }

            Logger.Debug("Retrieved {0} users", users.Count);

            return users;
        }
    }
}