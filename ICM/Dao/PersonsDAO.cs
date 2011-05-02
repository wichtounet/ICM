using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using ICM.Model;
using ICM.Utils;

namespace ICM.Dao
{
    /// <summary>
    ///  This class enables the user to make operations on the "Person" table. With this DAO, you can create, update, delete and search for persons. 
    /// </summary>
    public class PersonsDAO
    {
        /// <summary>
        /// Create a new person with the given values
        /// </summary>
        /// <param name="name">The name of the person</param>
        /// <param name="firstname">The name of the person</param>
        /// <param name="email">The name of the person</param>
        /// <param name="phone">The name of the person</param>
        /// <returns>the id of the inserted person</returns>
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

        /// <summary>
        /// Save the given person. Make a single UPDATE on the database with the id. 
        /// </summary>
        /// <param name="id">The id of the person to save</param>
        /// <param name="name">The name of the person</param>
        /// <param name="firstname">The name of the person</param>
        /// <param name="email">The name of the person</param>
        /// <param name="phone">The name of the person</param>
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

        /// <summary>
        /// Archive the person with the given id
        /// </summary>
        /// <param name="id">The id of the person to save</param>
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

        /// <summary>
        /// Search for persons in the database using the given arguments as criteria
        /// </summary>
        /// <param name="name">The name to search persons for</param>
        /// <param name="firstname">The first name to search persons for</param>
        /// <param name="archived">Indicate if we must search for the archived persons to</param>
        /// <returns>all the persons matching the criterias</returns>
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

        /// <summary>
        /// Return the person with the given id
        /// </summary>
        /// <param name="id">The id of the person to search</param>
        /// <returns>the person with the given ID or null if there is no person with this person</returns>
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

        /// <summary>
        /// Returns all the persons of the database. 
        /// </summary>
        /// <returns>a List containing all the persons</returns>
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

        /// <summary>
        /// Bind the SQL Result to a Person object
        /// </summary>
        /// <param name="result">The result of a SQL query</param>
        /// <returns>a new instance of Person with the values of the SQL Result</returns>
        private static Person BindPerson(SqlResult result)
        {
            var person = new Person
            {
                Id = result["id"].ToString().ToInt(),
                Name = result["name"].ToString(),
                FirstName = result["firstname"].ToString(),
                Email = result["email"].ToString(),
                Phone = result["phone"].ToString(),
                Archived = result["archived"].ToString().Equals("True") ? true : false
            };

            //TODO : Get department of person
            
            return person;
        }
    }
}