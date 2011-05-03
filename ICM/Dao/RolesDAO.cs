using System.Collections.Generic;
using System.Data;
using ICM.Model;
using ICM.Utils;

namespace ICM.Dao
{
    public class RolesDAO
    {
        public List<Role> GetAllRoles()
        {
            var roles = new List<Role>();

            using (var reader = DBUtils.ExecuteQuery("Select * from [Role]", IsolationLevel.ReadUncommitted))
            {
                while (reader.Read())
                {
                    roles.Add(BindRole(reader));
                }
            }

            return roles;
        }

        private static Role BindRole(SqlResult reader)
        {
            return new Role
            {
                Name = reader["name"].ToString()
            };
        }
    }
}