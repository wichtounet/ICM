using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICM.Model;
using System.Data.SqlClient;
using ICM.Utils;

namespace ICM.Dao
{
    public class LanguagesDAO
    {
        public List<Language> GetAllLanguages()
        {
            List<Language> languages = new List<Language>();

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            SqlCommand command = new SqlCommand("SELECT * FROM [Language]", connection, transaction);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Language language = new Language();
                    language.Name = reader["name"].ToString();
                    languages.Add(language);
                }
            }

            transaction.Commit();

            return languages;
        }
    }
}