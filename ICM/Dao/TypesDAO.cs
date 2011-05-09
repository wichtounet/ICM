using System.Collections.Specialized;
using System.Data.SqlClient;
using ICM.Model;
using ICM.Utils;
using System.Data;
using System.Collections.Generic;

namespace ICM.Dao
{
    public class TypesDAO
    {
        public List<TypeContract> GetAllTypes(SqlConnection connection)
        {
            var types = new List<TypeContract>();

            using (var reader = DBUtils.ExecuteQuery("SELECT name FROM [typeContract]", connection, IsolationLevel.ReadUncommitted, new NameValueCollection()))
            {
                while (reader.Read())
                {
                    types.Add(BindType(reader));
                }
            }

            return types;
        }

        private TypeContract BindType(SqlResult reader)
        {
            return new TypeContract {Name = reader["name"].ToString()};
        }
    }
}