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
                    institution = GetInstitutionWithoutDepartments(institutionReader);
                }
            }

            if(institution != null)
            {
                institution.Departments = GetDepartments(institution.Id, transaction);
            }
            
            DBUtils.CommitTransaction(transaction);

            return institution;
        }

        //TODO: test
        public void UpdateInstitution(Institution institution)
        {
            var parameters = new NameValueCollection
            {
                {"@id", institution.Id.ToString()},
                {"@name", institution.Name},
                {"@description", institution.Description},
                {"@city", institution.City},
                {"@interest", institution.Interest},
                {"@languageName", institution.Language.Name},
                {"@countryName", institution.Country.Name},
                {"@archived", "0"}
            };

            DBUtils.ExecuteUpdate(
                "UPDATE [Institution] SET name = @name, description = @description, city = @city, interest = @interest, languageName = @languageName, countryName = @countryName, WHERE id = @id",
                IsolationLevel.ReadUncommitted, parameters);
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
                    institutions.Add(GetInstitutionWithoutDepartments(institutionReader));
                }
            }

            //Add department list to the institutions
            foreach (Institution institution in institutions)
            {
                institution.Departments = GetDepartments(institution.Id, transaction);
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
                    departments.Add(new Department() { Name = departmentReader["name"].ToString() });
                }
            }

            return departments;
        }

        public void ArchiveInstitution(int id)
        {
            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()}
            };

            DBUtils.ExecuteUpdate(
                "UPDATE [Institution] SET countryName = 1, WHERE id = @id",
                IsolationLevel.ReadUncommitted, parameters);
        }

        private static Institution GetInstitutionWithoutDepartments(SqlResult institutionReader)
        {
            //Instantiate institution
            return new Institution( (int) institutionReader["id"],
                                    (string) institutionReader["name"],
                                    (string) institutionReader["description"],
                                    (string) institutionReader["city"],
                                    (string) institutionReader["Interest"],
                                    new Language { Name = (string) institutionReader["languageName"] },
                                    new Country { Name = (string) institutionReader["countryName"] }, 
                                    null,
                                    (bool) institutionReader["archived"]);
        }
    }
}