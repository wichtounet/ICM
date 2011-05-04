using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ICM.Model;
using ICM.Utils;

namespace ICM.Dao
{
    public class InstitutionsDAO
    {
        public int AddInstitution(Institution institution)
        {
            int institutionId;

            var parametersInstitution = new NameValueCollection
            {
                {"@name", institution.Name},
                {"@description", institution.Description},
                {"@city", institution.City},
                {"@interest", institution.Interest},
                {"@languageName", institution.Language.Name},
                {"@countryName", institution.Country.Name},
                {"@archived", "0"}
            };

            //TODO: is the right isolation level for christ's sake??? and check all the rest of the document!
            SqlTransaction transaction = DBUtils.BeginTransaction(IsolationLevel.ReadUncommitted);

            //Insert institution
            institutionId = DBUtils.ExecuteTransactionInsert(
                "INSERT INTO [Institution] (name,description,city,interest,languageName,countryName,archived) VALUES (@name,@description,@city,@interest,@languageName,@countryName,@archived)",
                transaction, parametersInstitution, "Institution");

            //Insert departments
            foreach (Department department in institution.Departments)
            {
                var parametersDepartment = new NameValueCollection
                {
                    {"@name", department.Name},
                    {"@institutionId", institutionId.ToString()},
                    {"@archived", "0"}
                };
                DBUtils.ExecuteTransactionInsert(
                    "INSERT INTO [Department] (name,institutionId,archived) VALUES (@name,@institutionId,@archived)",
                    transaction, parametersDepartment, "Department");
            }

            DBUtils.CommitTransaction(transaction);

            return institutionId;
        }

        public Institution GetInstitution(int id)
        {
            Institution institution = null;

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            var transaction = DBUtils.BeginTransaction(IsolationLevel.ReadUncommitted);

            using (var institutionReader = DBUtils.ExecuteTransactionQuery("SELECT * FROM [Institution] WHERE id = @id", transaction, parameters))
            {
                if(institutionReader.Read())
                {
                    institution = GetInstitutionWithoutDepartmentsAndContinent(institutionReader);
                }
            }

            if(institution != null)
            {
                institution.Departments = GetDepartments(institution.Id, transaction);
                institution.Country.Continent = GetContinent(institution.Country.Name, transaction);
            }
            
            DBUtils.CommitTransaction(transaction);

            return institution;
        }

        //TODO: test
        public void UpdateInstitution(Institution institution)
        {
            SqlConnection connection = DBManager.GetInstance().GetConnection();
            SqlTransaction transaction = DBUtils.BeginTransaction(IsolationLevel.ReadUncommitted);
            NameValueCollection parameters;

            List<Department> oldDepartments = GetDepartments(institution.Id, transaction);  //Department already present in DB

            //Delete all the old departments
            foreach(Department oldDepartment in oldDepartments)
            {
                if (!institution.Departments.ContainsDepartmentWithName(oldDepartment.Name))
                {
                    parameters = new NameValueCollection
                    {
                        {"@departmentId", oldDepartment.Id.ToString()},
                    };
                    DBUtils.ExecuteNonQuery(connection,
                        "DELETE FROM [Department] WHERE id=@departmentId",
                        transaction,
                        parameters);
                }
            }

            //Add all the new departments
            foreach(Department department in institution.Departments)
            {
                if (!oldDepartments.ContainsDepartmentWithName(department.Name))
                {
                    parameters = new NameValueCollection
                    {
                        {"@name", department.Name},
                        {"@institutionId", institution.Id.ToString()},
                        {"@archived", "0"}
                    };
                    DBUtils.ExecuteNonQuery(
                        connection,
                        "INSERT INTO [Department] (name,institutionId,archived) VALUES (@name,@institutionId,@archived)",
                        transaction,
                        parameters);
                }
            }

            //Update institution data
            parameters = new NameValueCollection
            {
                {"@institutionId", institution.Id.ToString()},
                {"@name", institution.Name},
                {"@description", institution.Description},
                {"@city", institution.City},
                {"@interest", institution.Interest},
                {"@languageName", institution.Language.Name},
                {"@countryName", institution.Country.Name},
                {"@archived", "0"}
            };
            DBUtils.ExecuteNonQuery(
                connection,
                "UPDATE [Institution] SET name = @name, description = @description, city = @city, interest = @interest, languageName = @languageName, countryName = @countryName WHERE id = @institutionId",
                transaction,
                parameters);

            //Commit transaction
            DBUtils.CommitTransaction(transaction);
        }

        public List<Institution> GetInstitutions()
        {
            List<Institution> institutions = new List<Institution>();
            SqlTransaction transaction = DBUtils.BeginTransaction(IsolationLevel.ReadUncommitted);

            //Instantiate institutions without department list
            using (SqlResult institutionReader = DBUtils.ExecuteTransactionQuery("SELECT * FROM [Institution]", transaction))
            {
                while (institutionReader.Read())
                {
                    institutions.Add(GetInstitutionWithoutDepartmentsAndContinent(institutionReader));
                }
            }

            //Add department list to the institutions
            foreach (Institution institution in institutions)
            {
                institution.Departments = GetDepartments(institution.Id, transaction);
                institution.Country.Continent = GetContinent(institution.Country.Name, transaction);
            }

            DBUtils.CommitTransaction(transaction);
            
            return institutions;
        }

        public List<Department> GetDepartments(int institutionId) {
            SqlTransaction transaction = DBUtils.BeginTransaction(IsolationLevel.ReadUncommitted);
            List<Department> departments = GetDepartments(institutionId, transaction);
            DBUtils.CommitTransaction(transaction);
            return departments;
        }

        public void ArchiveInstitution(int id)
        {
            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()}
            };

            DBUtils.ExecuteUpdate(
                "UPDATE [Institution] SET archived = 1 WHERE id = @id",
                IsolationLevel.ReadUncommitted, parameters);
        }

        private static Institution GetInstitutionWithoutDepartmentsAndContinent(SqlResult institutionReader)
        {
            //Instantiate institution
            return new Institution( (int) institutionReader["id"],
                                    (string) institutionReader["name"],
                                    (string) institutionReader["description"],
                                    (string) institutionReader["city"],
                                    (string) institutionReader["Interest"],
                                    new Language { Name = (string) institutionReader["languageName"] },
                                    new Country { Name = (string)institutionReader["countryName"] }, 
                                    null,
                                    (bool) institutionReader["archived"]);
        }

        private List<Department> GetDepartments(int institutionId, SqlTransaction transaction)
        {
            List<Department> departments = new List<Department>();

            var parameters = new NameValueCollection
            {
                {"@institutionId", institutionId.ToString()}
            };

            using (SqlResult departmentReader = DBUtils.ExecuteTransactionQuery("SELECT * FROM [Department] WHERE institutionId = @institutionId", transaction, parameters))
            {
                //Fill departments list
                while (departmentReader.Read())
                {
                    Department department = new Department();
                    department.Id = (int)departmentReader["id"];
                    department.Name = (string)departmentReader["name"];
                    departments.Add(department);
                }
            }

            return departments;
        }

        private Continent GetContinent(string countryName, SqlTransaction transaction)
        {
            //Get continent name
            string continentName;
            var parameters = new NameValueCollection
            {
                {"@countryName", countryName}
            };

            using (SqlResult countryReader = DBUtils.ExecuteTransactionQuery("SELECT * FROM [Country] WHERE name = @countryName", transaction, parameters))
            {
                countryReader.Read();
                continentName = (string)countryReader["continentName"];
            }

            //Instantiate continent
            return new Continent() { Name = continentName };
        }

        /// <summary>
        /// Search for institutions in the database using the given arguments as criteria
        /// </summary>
        /// <param name="name">Instution name</param>
        /// <param name="language">Institution language</param>
        /// <param name="continent">Institution country</param>
        /// <param name="country">Institution country</param>
        /// <param name="archived">Institution state (archived or not)</param>
        /// <returns>All the institutions matching the search criteria</returns>
        public List<Institution> SearchInstitutions(string name, string language, string continent, string country, bool archived)
        {
            List<Institution> institutions = new List<Institution>();
            var parameters = new NameValueCollection
            {
                {"@name", "%" + name + "%"},
                {"@languageName", "%" + language + "%"},
                {"@countryName", "%" + country + "%"},
                {"@continentName", continent}
            };

            // Create query string
            string query =  "SELECT * FROM [Institution] WHERE name LIKE(@name) AND languageName LIKE(@languageName)";
            
            if (!archived)
            {
                query += " AND archived = 0";
            }

            if (country != "")
            {
                query += " AND countryName LIKE(@countryName)";
            }
            else if (continent != "")
            {
                query += " AND countryName IN (SELECT name FROM country WHERE continentName = @continentName)";
            }

            //Start transaction
            SqlTransaction transaction = DBUtils.BeginTransaction(IsolationLevel.ReadUncommitted);

            //Instantiate institutions without department list
            using (SqlResult institutionReader = DBUtils.ExecuteTransactionQuery(query, transaction, parameters))
            {
                while (institutionReader.Read())
                {
                    institutions.Add(GetInstitutionWithoutDepartmentsAndContinent(institutionReader));
                }
            }

            //Add department list to the institutions
            foreach (Institution institution in institutions)
            {
                institution.Departments = GetDepartments(institution.Id, transaction);
                institution.Country.Continent = GetContinent(institution.Country.Name, transaction);
            }

            //End transaction
            DBUtils.CommitTransaction(transaction);

            return institutions;
        }
    }
}