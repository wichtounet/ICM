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
        public List<Language> getAllLanguages()
        {
            List<Language> languages = new List<Language>();

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlCommand command = new SqlCommand("Select * from [Language]", connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Language language = new Language();
                language.Name = reader[0].ToString();
                languages.Add(language);
            }

            return languages;
        }
    }
}