using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using ICM.Model;
using ICM.Utils;
using NLog;

namespace ICM.Dao
{
    /// <summary>
    ///  This class enables the user to search the database for countries and continents. 
    /// </summary>
    public class CountriesDAO
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns all the continents of the database. 
        /// </summary>
        /// <returns>a List containing all the contients</returns>
        public List<Continent> GetAllContinents()
        {
            Logger.Debug("Get all continents");

            var continents = new List<Continent>();

            using (var reader = DBUtils.ExecuteQuery("SELECT * FROM [Continent]", IsolationLevel.ReadUncommitted))
            {
                while (reader.Read())
                {
                    continents.Add(BindContinent(reader));
                }
            }

            Logger.Debug("Found {0} continents", continents.Count);

            return continents;
        }

        /// <summary>
        /// Returns all the countries of the specified continent of the database. 
        /// </summary>
        /// <param name="continent">The contient to search country for</param>
        /// <returns>a List containing all the countries of the specified continent</returns>
        public List<Country> GetCountries(Continent continent)
        {
            Logger.Debug("Search countries of {0}", continent.Name);

            var countries = new List<Country>();

            var parameters = new NameValueCollection
            {
                {"@continent", continent.Name}
            };

            using (var reader = DBUtils.ExecuteQuery("SELECT * FROM [Country] WHERE continentName = @continent", IsolationLevel.ReadUncommitted, parameters))
            {
                while (reader.Read())
                {
                    countries.Add(BindCountry(reader));
                }
            }

            Logger.Debug("Found {0} countries", countries.Count);

            return countries;
        }

        /// <summary>
        /// Bind the SQL Result to a Country object
        /// </summary>
        /// <param name="result">The result of a SQL query</param>
        /// <returns>a new instance of Country with the values of the SQL Result</returns>
        private static Country BindCountry(SqlResult result)
        {
            return new Country
            {
                Name = result["name"].ToString()
            };
        }

        /// <summary>
        /// Bind the SQL Result to a Continent object
        /// </summary>
        /// <param name="result">The result of a SQL query</param>
        /// <returns>a new instance of Continent with the values of the SQL Result</returns>
        private static Continent BindContinent(SqlResult result)
        {
            return new Continent
            {
                Name = result["name"].ToString()
            };
        }
    }
}