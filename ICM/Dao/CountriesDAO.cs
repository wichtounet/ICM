using System.Collections.Generic;
using ICM.Model;
using System.Data.SqlClient;
using ICM.Utils;

namespace ICM.Dao
{
    public class CountriesDAO
    {
        public List<Continent> GetAllContinents()
        {
            var continents = new List<Continent>();

            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            var command = new SqlCommand("SELECT * FROM [Continent]", connection, transaction);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    continents.Add(BindContinent(reader));
                }
            }

            transaction.Commit();

            return continents;
        }

        public List<Country> GetCountries(Continent continent)
        {
            var countries = new List<Country>();

            var connection = DBManager.GetInstance().GetConnection();

            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            var command = new SqlCommand("SELECT * FROM [Country] WHERE continentName = @continent", connection, transaction);
            command.Parameters.AddWithValue("@continent", continent.Name);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    countries.Add(BindCountry(reader));
                }
            }

            transaction.Commit();

            return countries;
        }

        private static Country BindCountry(SqlDataReader reader)
        {
            return new Country
            {
                Name = reader["name"].ToString()
            };
        }

        private static Continent BindContinent(SqlDataReader reader)
        {
            return new Continent
            {
                Name = reader["name"].ToString()
            };
        }
    }
}