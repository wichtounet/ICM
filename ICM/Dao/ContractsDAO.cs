using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICM.Model;
using ICM.Utils;
using System.Data.SqlClient;

namespace ICM.Dao
{
    public class ContractsDAO
    {
        public Contract GetContractById(int id)
        {
            Contract contract = new Contract();

            return contract;
        }

        public List<Contract> GetContractsBySearch(string title, string type, int personId, int departmentId, int destinationId, int institutionId)
        {
            List<Contract> contractsList = new List<Contract>();

            
            return contractsList;
        }

        public List<Contract> GetAllContracts()
        {
            List<Contract> contractsList = new List<Contract>();

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction();
            //transaction.IsolationLevel = System.Data.IsolationLevel.Chaos;

            SqlCommand command = new SqlCommand("Select * from [Contract]", connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Contract contract = new Contract();
                contract.Title = reader[0].ToString();
                contractsList.Add(contract);
            }

            reader.Close();

            return contractsList;
        } 
    }
}