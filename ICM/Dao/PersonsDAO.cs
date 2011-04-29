using System.Collections.Generic;
using System.Linq;
using ICM.Model;
using System.Data.SqlClient;
using ICM.Utils;

namespace ICM.Dao
{
    public class PersonsDAO
    {
        public void CreatePerson(string firstname, string name, string phone, string email)
        {
            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            var command = new SqlCommand("INSERT INTO [Person] (firstname,name,phone,email,archived,departmentId) VALUES (@firstname,@name,@phone,@email,@archived,@department)", connection, transaction);
            command.Parameters.AddWithValue("@firstname", firstname);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@archived", 0);
            command.Parameters.AddWithValue("@department", 73);

            command.ExecuteNonQuery();

            transaction.Commit();
        }

        public void ArchivePerson(int person)
        {
            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            var command = new SqlCommand("UPDATE [Person] SET archived = 1 WHERE id = @id", connection, transaction);
            command.Parameters.AddWithValue("@id", person);

            command.ExecuteNonQuery();

            transaction.Commit();
        }

        public List<Person> SearchPersons(string name, string firstname, bool archived)
        {
            var persons = new List<Person>();

            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            string query = "SELECT * FROM [Person] WHERE name LIKE(@name) AND firstname LIKE(@firstname)";

            if (!archived)
            {
                query += " AND archived = 0";
            }

            var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@name", "%" + name + "%");
            command.Parameters.AddWithValue("@firstname", "%" + firstname + "%");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    persons.Add(BindPerson(reader));
                }
            }

            transaction.Commit();

            return persons;
        }

        public Person GetPersonByID(int id)
        {
            var persons = new List<Person>();

            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            var command = new SqlCommand("SELECT * FROM [Person] WHERE id = @id", connection, transaction);
            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    persons.Add(BindPerson(reader));
                }
            }

            transaction.Commit();

            return persons.First();
        }

        public List<Person> GetAllPersons()
        {
            var persons = new List<Person>();

            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            var command = new SqlCommand("SELECT * FROM [Person]", connection, transaction);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    persons.Add(BindPerson(reader));
                }
            }

            transaction.Commit();

            return persons;
        }

        private static Person BindPerson(SqlDataReader reader)
        {
            var person = new Person
            {
                Id = reader["id"].ToString().ToInt(),
                Name = reader["name"].ToString(),
                FirstName = reader["firstname"].ToString(),
                Email = reader["email"].ToString(),
                Phone = reader["phone"].ToString(),
                Archived = reader["archived"].ToString().Equals("1") ? true : false
            };

            //TODO : Get department of person
            
            return person;
        }
    }
}