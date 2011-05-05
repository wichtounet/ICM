using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using ICM.Model;
using ICM.Dao;
using ICM.Utils;

namespace ICM.Dao
{
    public class UsersDAO
    {
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            SqlResult reader;

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

            return users;
        }
    }
}