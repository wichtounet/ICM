using System.Collections.Generic;
using ICM.Model;
using System.Data.SqlClient;
using ICM.Utils;

namespace ICM.Dao
{
    public class RolesDAO
    {
        public List<Role> GetAllRoles()
        {
            var roles = new List<Role>();

            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            var command = new SqlCommand("Select * from [Role]", connection, transaction);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    roles.Add(BindRole(reader));
                }
            }

            transaction.Commit();

            return roles;
        }

        private static Role BindRole(SqlDataReader reader)
        {
            return new Role
            {
                Name = reader["name"].ToString()
            };
        }
    }
}