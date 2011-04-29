using System.Collections.Generic;
using ICM.Model;
using System.Data.SqlClient;
using ICM.Utils;

namespace ICM.Dao
{
    public class LanguagesDAO
    {
        public List<Language> GetAllLanguages()
        {
            var languages = new List<Language>();

            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            var command = new SqlCommand("SELECT * FROM [Language]", connection, transaction);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    languages.Add(BindLanguage(reader));
                }
            }

            transaction.Commit();

            return languages;
        }

        private static Language BindLanguage(SqlDataReader reader)
        {
            return new Language {Name = reader["name"].ToString()};
        }
    }
}