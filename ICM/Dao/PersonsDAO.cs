using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ICM.Model;
using ICM.Utils;
using NLog;

namespace ICM.Dao
{
    /// <summary>
    ///  This class enables the user to make operations on the "Person" table. With this DAO, you can create, update, delete and search for persons. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public class PersonsDAO
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Create a new person with the given values. This method creates a new connection and close it. 
        /// </summary>
        /// <param name="name">The name of the person</param>
        /// <param name="firstname">The firstname of the person</param>
        /// <param name="email">The email of the person</param>
        /// <param name="phone">The phone of the person</param>
        /// <param name="department">The department of the person</param>
        /// <returns>the id of the inserted person</returns>
        public int CreatePerson(string firstname, string name, string phone, string email, int department)
        {
            using (var connection = DBManager.GetInstance().GetNewConnection())
            {
                var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                Logger.Debug("Creating person");

                var parameters = new NameValueCollection
                {
                    {"@firstname", firstname},
                    {"@name", name},
                    {"@phone", phone},
                    {"@email", email},
                    {"@archived", "0"},
                    {"@department", department.ToString()}
                };

                var id = DBUtils.ExecuteInsert(
                    "INSERT INTO [Person] (firstname,name,phone,email,archived,departmentId) VALUES (@firstname,@name,@phone,@email,@archived,@department)", parameters, "Person", transaction);

                transaction.Commit();

                Logger.Debug("Created person with id {0}", id);

                return id;
            }
        }

        /// <summary>
        /// Save the given person. Make a single UPDATE on the database with the id. 
        /// </summary>
        /// <param name="id">The id of the person to save</param>
        /// <param name="firstname">The name of the person</param>
        /// <param name="name">The name of the person</param>
        /// <param name="phone">The name of the person</param>
        /// <param name="email">The name of the person</param>
        /// <param name="department">The department of the person</param>
        /// <param name="transaction">The transaction to use</param>
        public void SavePerson(int id, string firstname, string name, string phone, string email, int department, SqlTransaction transaction)
        {
            Logger.Debug("Saving person {0}", id);

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
                {"@firstname", firstname},
                {"@name", name},
                {"@phone", phone},
                {"@email", email},
                {"@department", department.ToString()},
            };

            DBUtils.ExecuteNonQuery(
                "UPDATE [Person] SET firstname = @firstname, name = @name, phone = @phone, email = @email, departmentId = @department WHERE id = @id",
                transaction, parameters);

            Logger.Debug("Saved person {0}", id);
        }

        /// <summary>
        /// Archive the person with the given id. This method open a new connection and close it
        /// </summary>
        /// <param name="id">The id of the person to save</param>
        public void ArchivePerson(int id)
        {
            Logger.Debug("Archiving person {0}", id);

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            DBUtils.ExecuteUpdate(
                "UPDATE [Person] SET archived = 1 WHERE id = @id",
                IsolationLevel.ReadUncommitted, parameters);

            Logger.Debug("Archived person {0}", id);
        }

        /// <summary>
        /// Search for persons in the database using the given arguments as criteria. This method open a new connection and close it. 
        /// </summary>
        /// <param name="name">The name to search persons for</param>
        /// <param name="firstname">The first name to search persons for</param>
        /// <param name="archived">Indicate if we must search for the archived persons to</param>
        /// <param name="institution">The institution we must search for, or -1 if this is not a criteria</param>
        /// <param name="department">The department we must search for, or -1 if this is not a criteria</param>
        /// <returns>all the persons matching the criterias</returns>
        public List<Person> SearchPersons(string name, string firstname, bool archived, int institution, int department)
        {
            var persons = new List<Person>();
            
            var query = GetBaseQuery();

            query += " WHERE P.name LIKE(@name) AND P.firstname LIKE(@firstname)";

            if (!archived)
            {
                query += " AND P.archived = 0";
            }

            if(department != -1)
            {
                query += " AND P.departmentId = @department";
            } 
            else if(institution != -1)
            {
                query += " AND D.institutionId = @institution";
            }

            var parameters = new NameValueCollection
            {
                {"@name", "%" + name + "%"},
                {"@firstname", "%" + firstname + "%"},
                {"@department", department.ToString()},
                {"@institution", institution.ToString()},
            };

            Logger.Debug("Searching persons, with query {0}", query);

            using(var connection = DBManager.GetInstance().GetNewConnection())
            {
                using (var reader = DBUtils.ExecuteQuery(query, connection, IsolationLevel.ReadUncommitted, parameters))
                {
                    while (reader.Read())
                    {
                        persons.Add(BindPerson(reader));
                    }
                }
            }

            Logger.Debug("Found {0} persons", persons.Count);

            return persons;
        }

        private static string GetBaseQuery()
        {
            return "SELECT DISTINCT P.id, P.name, P.firstname, P.email, P.phone, P.archived, P.departmentId, D.name AS departmentName, I.name AS institutionName, I.id AS institutionId " +
                   "FROM Person P " + 
                   "INNER JOIN Department D ON P.departmentId = D.id " + 
                   "INNER JOIN Institution I ON D.institutionId = I.id";
        }

        /// <summary>
        /// Return the person with the given id. This method open a new connection and close it
        /// </summary>
        /// <param name="id">The id of the person to search</param>
        /// <returns>the person with the given ID or null if there is no person with this person</returns>
        public Person GetPersonByID(int id)
        {
            using (var connection = DBManager.GetInstance().GetNewConnection())
            {
                return GetPersonByID(id, connection);
            }
        }

        ///<summary>
        /// Return the person with the given id. 
        ///</summary>
        /// <param name="id">The id of the person to search</param>
        ///<param name="connection">The connection to use. </param>
        /// <returns>the person with the given ID or null if there is no person with this person</returns>
        public Person GetPersonByID(int id, SqlConnection connection)
        {
            var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

            var person = GetPersonByID(id, transaction);

            transaction.Commit();

            return person;
        }

        ///<summary>
        /// Return the person with the given id. 
        ///</summary>
        /// <param name="id">The id of the person to search</param>
        ///<param name="transaction">The transactiont to use</param>
        /// <returns>the person with the given ID or null if there is no person with this person</returns>
        public Person GetPersonByID(int id, SqlTransaction transaction)
        {
            Logger.Debug("Search person by ID ({0})", id);

            var persons = new List<Person>();

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            using (var reader = DBUtils.ExecuteTransactionQuery(GetBaseQuery() + " WHERE P.id = @id", transaction, parameters))
            {
                while (reader.Read())
                {
                    persons.Add(BindPerson(reader));
                }
            }

            if(persons.Count > 0)
            {
                var person = persons.First();

                Logger.Debug("Found {0}", person.ToString());

                return person;
            }

            Logger.Debug("Found no person");

            return null;
        }

        ///<summary>
        /// Lock a person. It seems that this person cannot be update anymore by an other transaction. 
        ///</summary>
        ///<param name="id">The id of the person. </param>
        ///<param name="transaction">The transaction to use. </param>
        public void LockPerson(int id, SqlTransaction transaction)
        {
            Logger.Debug("Lock Person with ID ({0})", id);

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            var command = new SqlCommand("UPDATE PERSON set name = name WHERE id = @id", transaction.Connection, transaction){CommandTimeout = 3};

            foreach (var key in parameters.AllKeys)
            {
                command.Parameters.AddWithValue(key, parameters.Get(key));
            }

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Returns all the persons of the database. 
        /// </summary>
        /// <returns>a List containing all the persons</returns>
        public List<Person> GetAllPersons(SqlConnection connection)
        {
            Logger.Debug("Get all persons");

            var persons = new List<Person>();

            using (var reader = DBUtils.ExecuteQuery(GetBaseQuery(), connection, IsolationLevel.ReadUncommitted, new NameValueCollection()))
            {
                while (reader.Read())
                {
                    persons.Add(BindPerson(reader));
                }
            }

            Logger.Debug("Found {0} persons", persons.Count);

            return persons;
        }

        ///<summary>
        /// Make a search of persons for historique
        ///</summary>
        ///<param name="transaction">The transaction to use</param>
        ///<param name="contracts">All the contracts we want the persons for. </param>
        ///<returns>All the persons related with the given contracts</returns>
        public List<Person> HistoSearch(SqlTransaction transaction, List<Contract> contracts)
        {
            var persons = new List<Person>();

            var query = GetBaseQuery();
            query += " INNER JOIN Association A ON A.person = P.id";
            query += " WHERE A.roleName = 'Etudiant' AND A.contractId IN (";

            if(contracts.Count > 0)
            {
                query += contracts[0].Id;

                for(var i = 1; i < contracts.Count; i++)
                {
                    query += ", " + contracts[i].Id;
                }
            }

            query += ")";

            Logger.Debug("Histo search using query \"{0}\"", query);

            using (var reader = DBUtils.ExecuteTransactionQuery(query, transaction, new NameValueCollection()))
            {
                while (reader.Read())
                {
                    persons.Add(BindPerson(reader));
                }
            }

            Logger.Debug("Found {0} persons", persons.Count);

            return persons;
        }

        /// <summary>
        /// Bind the SQL Result to a Person object
        /// </summary>
        /// <param name="result">The result of a SQL query</param>
        /// <returns>a new instance of Person with the values of the SQL Result</returns>
        private static Person BindPerson(SqlResult result)
        {
            var department = new Department
            {
                Id = (int) result["departmentId"],
                Name = (string) result["departmentName"],
                InstitutionName = (string) result["institutionName"],
                InstitutionId = (int) result["institutionId"]
            };

            var person = new Person
            {
                Id = (int) result["id"],
                Name = (string) result["name"],
                FirstName = (string) result["firstname"],
                Email = (string) result["email"],
                Phone = (string) result["phone"],
                Archived = (bool) result["archived"],
                Department = department
            };

            return person;
        }
    }
}