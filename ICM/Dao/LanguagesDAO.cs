using System.Collections.Generic;
using System.Data;
using ICM.Model;
using ICM.Utils;
using NLog;

namespace ICM.Dao
{
    /// <summary>
    ///  This class enables the user to search the database for languages. 
    /// </summary>
    public class LanguagesDAO
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns all the languages of the database. 
        /// </summary>
        /// <returns>a List containing all the languages</returns>
        public List<Language> GetAllLanguages()
        {
            Logger.Debug("Get all languages");

            var languages = new List<Language>();

            using (var reader = DBUtils.ExecuteQuery("SELECT * FROM [Language]", IsolationLevel.ReadUncommitted))
            {
                while (reader.Read())
                {
                    languages.Add(BindLanguage(reader));
                }
            }

            Logger.Debug("Found {0} languages", languages.Count);

            return languages;
        }

        /// <summary>
        /// Bind the SQL Result to a Language object
        /// </summary>
        /// <param name="result">The result of a SQL query</param>
        /// <returns>a new instance of Language with the values of the SQL Result</returns>
        private static Language BindLanguage(SqlResult result)
        {
            return new Language
            {
                Name = result["name"].ToString()
            };
        }
    }
}