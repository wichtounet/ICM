using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICM.Model;
using System.Data.SqlClient;
using ICM.Utils;

namespace ICM.Dao
{
    public class CountriesDAO
    {
        public List<Continent> GetAllContinents()
        {
            List<Continent> continents = new List<Continent>();

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            SqlCommand command = new SqlCommand("Select * from [Continent]", connection, transaction);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Continent continent = new Continent();
                    continent.Name = reader[0].ToString();
                    continents.Add(continent);
                }
            }

            transaction.Commit();

            return continents;
        }

        public List<Country> GetCountries(Continent continent)
        {
            List<Country> countries = new List<Country>();

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            SqlCommand command = new SqlCommand("Select * from [Country] Where continentName = @continent", connection, transaction);
            command.Parameters.AddWithValue("@continent", continent.Name);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Country country = new Country();
                    country.Name = reader[0].ToString();
                    countries.Add(country);
                }
            }

            transaction.Commit();

            return countries;
        }
    }
}