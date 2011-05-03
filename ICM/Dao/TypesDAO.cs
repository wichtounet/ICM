using ICM.Model;
using ICM.Utils;
using System.Data;
using System.Collections.Generic;

namespace ICM.Dao
{
    public class TypesDAO
    {
        public List<TypeContract> GetAllPersons()
        {
            var types = new List<TypeContract>();

            using (var reader = DBUtils.ExecuteQuery("SELECT name FROM [typeContract]", IsolationLevel.ReadUncommitted))
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
            TypeContract type = new TypeContract();

            type.Name= reader["name"].ToString();

            return type;
        }
    }
}