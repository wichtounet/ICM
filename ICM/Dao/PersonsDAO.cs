using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICM.Model;
using System.Data.SqlClient;
using ICM.Utils;

namespace ICM.Dao
{
    public class PersonsDAO
    {
        public void ArchivePerson(int person)
        {
            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            SqlCommand command = new SqlCommand("UPDATE [Person] SET archived = 1 WHERE id = @id", connection, transaction);
            command.Parameters.AddWithValue("@id", person);

            command.ExecuteNonQuery();

            transaction.Commit();
        }

        public List<Person> SearchPersons(string name, string firstname, bool archived)
        {
            List<Person> persons = new List<Person>();

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            string query = "SELECT * FROM [Person] WHERE name LIKE(@name) AND firstname LIKE(@firstname)";

            if (!archived)
            {
                query += " AND archived = 0";
            }

            SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@name", "%" + name + "%");
            command.Parameters.AddWithValue("@firstname", "%" + firstname + "%");

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Person person = BindPerson(reader);
                    persons.Add(person);
                }
            }

            transaction.Commit();

            return persons;
        }

        public Person GetPersonByID(int id)
        {
            List<Person> persons = new List<Person>();

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            SqlCommand command = new SqlCommand("SELECT * FROM [Person] WHERE id = @id", connection, transaction);
            command.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Person person = BindPerson(reader);
                    persons.Add(person);
                }
            }

            transaction.Commit();

            return persons.First();
        }

        public List<Person> GetAllPersons()
        {
            List<Person> persons = new List<Person>();

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            SqlCommand command = new SqlCommand("SELECT * FROM [Person]", connection, transaction);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Person person = BindPerson(reader);
                    persons.Add(person);
                }
            }

            transaction.Commit();

            return persons;
        }

        private static Person BindPerson(SqlDataReader reader)
        {
            Person person = new Person();

            person.Id = reader["id"].ToString().ToInt();
            person.Name = reader["name"].ToString();
            person.FirstName = reader["firstname"].ToString();
            person.Email = reader["email"].ToString();
            person.Phone = reader["phone"].ToString();
            person.Archived = reader["archived"].ToString().Equals("1") ? true : false;

            //TODO : Get department of person
            
            return person;
        }
    }
}