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

            //TODO: transaction needed (what about department insertion fails???)

            //Insert institution
            institutionId = DBUtils.ExecuteInsert(
                "INSERT INTO [Institution] (name,description,city,interest,languageName,countryName,archived) VALUES (@name,@description,@city,@interest,@languageName,@countryName,@archived)",
                IsolationLevel.ReadUncommitted, parametersInstitution, "Institution");

            //Insert departments
            foreach (Department department in institution.Departments)
            {
                var parametersDepartment = new NameValueCollection
                {
                    {"@name", department.Name},
                    {"@institutionId", institutionId.ToString()},
                    {"@archived", "0"}
                };
                DBUtils.ExecuteInsert(
                    "INSERT INTO [Department] (name,institutionId,archived) VALUES (@name,@institutionId,@archived)",
                    IsolationLevel.ReadUncommitted, parametersDepartment, "Department");
            }

            return institutionId;
        }

        //TODO: test
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
                    institution = BindInstitution(institutionReader);
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
                    institutions.Add(BindInstitution(institutionReader));
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
            List<Department> departments = new List<Department>();

            var parameters = new NameValueCollection
            {
                {"@institutionId", institutionId.ToString()}
            };

            using (SqlResult departmentReader = DBUtils.ExecuteQuery("SELECT * FROM [Department] WHERE institutionId = @institutionId", IsolationLevel.ReadUncommitted, parameters))
            {
                //Fill departments list
                while (departmentReader.Read())
                {
                    departments.Add(new Department() { Name = departmentReader["name"].ToString() });
                }
            }

            return departments;
        }

        public List<Department> GetDepartments(int institutionId, SqlTransaction transaction)
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

        private static Institution BindInstitution(SqlResult institutionReader)
        {
            //Instantiate institution
            return new Institution(institutionReader["id"].ToString().ToInt(),
                                    institutionReader["name"].ToString(),
                                    institutionReader["description"].ToString(),
                                    institutionReader["city"].ToString(),
                                    institutionReader["Interest"].ToString(),
                                    new Language { Name = institutionReader["languageName"].ToString() },
                                    new Country { Name = institutionReader["countryName"].ToString() }, 
                                    null);
        }
    }
}