using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using ICM.Model;
using ICM.Utils;

namespace ICM.Dao
{
    public class PersonsDAO
    {
        public int CreatePerson(string firstname, string name, string phone, string email)
        {
            var parameters = new NameValueCollection
            {
                {"@firstname", firstname},
                {"@name", name},
                {"@phone", phone},
                {"@email", email},
                {"@archived", "0"},
                {"@department", "73"}
            };

            return DBUtils.ExecuteInsert(
                "INSERT INTO [Person] (firstname,name,phone,email,archived,departmentId) VALUES (@firstname,@name,@phone,@email,@archived,@department)",
                IsolationLevel.ReadUncommitted, parameters, "Person");
        }

        public void SavePerson(int id, string firstname, string name, string phone, string email)
        {
            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
                {"@firstname", firstname},
                {"@name", name},
                {"@phone", phone},
                {"@email", email},
            };

            DBUtils.ExecuteUpdate(
                "UPDATE [Person] SET firstname = @firstname, name = @name, phone = @phone, email = @email WHERE id = @id",
                IsolationLevel.ReadUncommitted, parameters);
        }

        public void ArchivePerson(int id)
        {
            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            DBUtils.ExecuteUpdate(
                "UPDATE [Person] SET archived = 1 WHERE id = @id",
                IsolationLevel.ReadUncommitted, parameters);
        }

        public List<Person> SearchPersons(string name, string firstname, bool archived)
        {
            var persons = new List<Person>();

            var query = "SELECT * FROM [Person] WHERE name LIKE(@name) AND firstname LIKE(@firstname)";

            if (!archived)
            {
                query += " AND archived = 0";
            }

            var parameters = new NameValueCollection
            {
                {"@name", "%" + name + "%"},
                {"@firstname", "%" + firstname + "%"},
            };

            using (var reader = DBUtils.ExecuteQuery(query, IsolationLevel.ReadUncommitted, parameters))
            {
                while (reader.Read())
                {
                    persons.Add(BindPerson(reader));
                }
            }

            return persons;
        }

        public Person GetPersonByID(int id)
        {
            var persons = new List<Person>();

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            using (var reader = DBUtils.ExecuteQuery("SELECT * FROM [Person] WHERE id = @id", IsolationLevel.ReadUncommitted, parameters))
            {
                while (reader.Read())
                {
                    persons.Add(BindPerson(reader));
                }
            }

            return persons.First();
        }

        public List<Person> GetAllPersons()
        {
            var persons = new List<Person>();

            using (var reader = DBUtils.ExecuteQuery("SELECT * FROM [Person]", IsolationLevel.ReadUncommitted))
            {
                while (reader.Read())
                {
                    persons.Add(BindPerson(reader));
                }
            }

            return persons;
        }

        private static Person BindPerson(SqlResult reader)
        {
            var person = new Person
            {
                Id = reader["id"].ToString().ToInt(),
                Name = reader["name"].ToString(),
                FirstName = reader["firstname"].ToString(),
                Email = reader["email"].ToString(),
                Phone = reader["phone"].ToString(),
                Archived = reader["archived"].ToString().Equals("True") ? true : false
            };

            //TODO : Get department of person
            
            return person;
        }
    }
}