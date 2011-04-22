using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICM.Model;
using System.Data.SqlClient;
using ICM.Utils;

namespace ICM.Dao
{
    public class RolesDAO
    {
        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            SqlCommand command = new SqlCommand("Select * from [Role]", connection, transaction);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Role role = new Role();
                    role.Name = reader["name"].ToString();
                    roles.Add(role);
                }
            }

            transaction.Commit();

            return roles;
        }
    }
}