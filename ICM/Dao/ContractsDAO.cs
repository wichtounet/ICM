using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using ICM.Model;
using ICM.Utils;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Globalization;

namespace ICM.Dao
{
    public class ContractsDAO
    {
        public Contract GetContractById(int id)
        {
            var contracts = new List<Contract>();

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            using (var reader = DBUtils.ExecuteQuery("SELECT * FROM [Contract] WHERE id = @id", IsolationLevel.ReadUncommitted, parameters))
            {
                while (reader.Read())
                {
                    contracts.Add(BindContract(reader));
                }
            }

            return contracts.First();
        }

        public void GetContractFile(HttpContext context, int id)
        {
            try
            {
                var parameters = new NameValueCollection
                {
                    {"@id", id.ToString()}
                };

                using (var reader = DBUtils.ExecuteQuery("SELECT * FROM [ContractFile] WHERE id = @id", IsolationLevel.ReadUncommitted, parameters))
                {
                    if (reader.Read())
                    {
                        context.Response.ClearHeaders();
                        context.Response.ClearContent();
                        context.Response.AppendHeader("Pragma", "no-cache");
                        context.Response.AppendHeader("Cache-Control", "no-cache");
                        context.Response.CacheControl = "no-cache";
                        context.Response.Expires = -1;
                        context.Response.ExpiresAbsolute = new DateTime(1900, 1, 1);
                        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        context.Response.ContentType = reader["fileMIMEType"].ToString();
                        context.Response.BinaryWrite((byte[])reader["fileBinaryData"]);
                        context.Response.Flush();
                        context.Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.ToString());
            } 
        }

        public List<Contract> GetContractsBySearch(string title, string description, string type, int personId, int departmentId, int destinationId, int institutionId)
        {
            List<Contract> contractsList = new List<Contract>();
            /*
            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            string query = "SELECT * FROM [Contract] WHERE title LIKE(@title) AND description LIKE(@description)";

            SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@name", "%" + title + "%");
            command.Parameters.AddWithValue("@firstname", "%" + description + "%");

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Contract contract = BindContract(reader);
                    contractsList.Add(contract);
                }
            }

            transaction.Commit();
            */
            return contractsList;
        }

        public int addContract(string title, string start, string end, string typeContractName, string userName, int departmentId, int destinationId, int institutionId, int fileSize, string fileMIMEType, System.IO.BinaryReader fileBinaryReader, byte[] fileBinaryBuffer)
        {
            int contractFileId = addFile(fileSize, fileMIMEType, fileBinaryReader, fileBinaryBuffer);
            
            var parameters = new NameValueCollection
            {
                {"@title", title},
                {"@start", start},
                {"@end", end},
                {"@fileId", contractFileId.ToString()},
                {"@xmlContent", ""},
                {"@userLogin", userName},
                {"@typeContractName", typeContractName},
                {"@archived", "0"}
            };
            
            return DBUtils.ExecuteInsert(
                "INSERT INTO [Contract] (title, start, [end], fileId, xmlContent, userLogin, typeContractName, archived) VALUES (@title, @start, @end, @fileId, @xmlContent, @userLogin, @typeContractName, @archived)",
                IsolationLevel.ReadUncommitted, parameters, "Contract");
        }

        public List<Contract> GetAllContracts()
        {
            List<Contract> contractsList = new List<Contract>();
            /*
            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction();
            //transaction.IsolationLevel = System.Data.IsolationLevel.Chaos;

            SqlCommand command = new SqlCommand("Select * from [Contract]", connection);

            SqlResult reader = command.ExecuteReader();

            while (reader.Read())
            {
                Contract contract = BindContract(reader);
                contractsList.Add(contract);
            }

            reader.Close();
            */
            return contractsList;
        }
        
        private int addFile(int fileSize, string fileMIMEType, System.IO.BinaryReader fileBinaryReader, byte[] fileBinaryBuffer)
        {

            SqlConnection connection = DBManager.GetInstance().GetConnection();

            SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            string fileToDbQueryStr = @"INSERT INTO ContractFile (fileSize, fileMIMEType, fileBinaryData) VALUES(@fileSize, @fileMIMEType, @fileBinaryData)";
            System.Data.SqlClient.SqlCommand command = new SqlCommand(fileToDbQueryStr, connection, transaction);

            command.Parameters.AddWithValue("fileSize", fileSize);
            command.Parameters.AddWithValue("fileMIMEType", fileMIMEType);

            SqlParameter imageFileBinaryParam = new SqlParameter("fileBinaryData", SqlDbType.VarBinary, fileSize);
            imageFileBinaryParam.Value = fileBinaryBuffer;

            command.Parameters.Add(imageFileBinaryParam);

            command.ExecuteNonQuery();

            var getCommand = new SqlCommand("SELECT IDENT_CURRENT('ContractFile') AS ID", connection, transaction);

            int id;
            using (var reader = getCommand.ExecuteReader())
            {
                reader.Read();

                id = reader["ID"].ToString().ToInt();
            }

            transaction.Commit();

            return id;
        }

        private static Contract BindContract(SqlResult reader)
        {
            Contract contract = new Contract();

            contract.Id = reader["id"].ToString().ToInt();
            contract.Title = reader["title"].ToString();
            contract.Start = reader["start"].ToString();//DateTime.ParseExact(reader["start"].ToString(), "yyyy-MM-dd hh:mm:ss", null);
            contract.End = reader["end"].ToString();//DateTime.ParseExact(reader["end"].ToString(), "yyyy-MM-dd hh:mm:ss", null);
            contract.XmlContent = reader["xmlContent"].ToString();
            contract.User = reader["userLogin"].ToString();
            contract.Type = reader["typeContractName"].ToString();
            contract.Archived = reader["archived"].ToString().Equals("1") ? true : false;
            contract.fileId = reader["fileId"].ToString().ToInt();

            return contract;
        }
    }
}