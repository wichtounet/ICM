using System.Collections.Generic;
using System.Data;
using ICM.Model;
using ICM.Utils;

namespace ICM.Dao
{
    public class LanguagesDAO
    {
        public List<Language> GetAllLanguages()
        {
            var languages = new List<Language>();

            using (var reader = DBUtils.ExecuteQuery("SELECT * FROM [Language]", IsolationLevel.ReadUncommitted))
            {
                while (reader.Read())
                {
                    languages.Add(BindLanguage(reader));
                }
            }

            return languages;
        }

        private static Language BindLanguage(SqlResult reader)
        {
            return new Language
            {
                Name = reader["name"].ToString()
            };
        }
    }
}