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
using System.Collections;
using NLog;

namespace ICM.Dao
{
    public class ContractsDAO
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Contract GetContractById(int id)
        {
            var transaction = DBUtils.BeginTransaction(IsolationLevel.ReadUncommitted);

            var contract = GetContractById(id, transaction);

            transaction.Connection.Close();
            
            return contract;
        }


        //COPIED BY WICHT
        public Contract GetContractById(int id, SqlTransaction transaction)
        {
            var contracts = new List<Contract>();

            var parameters = new NameValueCollection
           {
               {"@id", id.ToString()},
           };

            Contract c = new Contract();
            List<Person> persons = new List<Person>();
            List<Department> departments = new List<Department>();

            using (var reader = DBUtils.ExecuteTransactionQuery("SELECT C.id, C.title, C.start, C.[end], C.fileId, C.xmlContent, C.userLogin, C.typeContractName, C.archived " +
                                                     "FROM [Contract] C " +
                                                     "WHERE C.id = @id"
                                                    , transaction, parameters))
            {

                if (reader.Read())
                {
                    c = BindContract(reader);
                }
            }

            using (var readerAssoc = DBUtils.ExecuteTransactionQuery("SELECT A.roleName, P.id AS personId, P.name, P.firstname " +
                                                     "FROM [Association] A " +
                                                     "INNER JOIN Person P " +
                                                     "ON A.person = P.id " +
                                                     "WHERE A.contractId = @id"
                                                    , transaction, parameters))
            {
                while (readerAssoc.Read())
                {
                    Person p = new Person();
                    p.Id = (int)readerAssoc["personId"];
                    p.Role = (string)readerAssoc["roleName"];
                    p.FirstName = (string)readerAssoc["firstname"];
                    p.Name = (string)readerAssoc["name"];
                    persons.Add(p);
                }
            }
            using (var readerDestination = DBUtils.ExecuteTransactionQuery("SELECT P.id AS departementId, P.name AS depName, I.id AS institutionId, I.name AS insName" +
                                                     " FROM [Destination] D" +
                                                     " INNER JOIN [Department] P" +
                                                     " ON D.department = P.id" +
                                                     " INNER JOIN [Institution] I" +
                                                     " ON P.institutionId = I.id" +
                                                     " WHERE D.contract = @id"
                                                    , transaction, parameters))
            {
                while (readerDestination.Read())
                {
                    Department d = new Department();
                    d.Id = (int)readerDestination["departementId"];
                    d.Name = (string)readerDestination["depName"];
                    d.InstitutionName = (string)readerDestination["insName"];
                    d.InstitutionId = (int)readerDestination["institutionId"];
                    departments.Add(d);
                }
            }
            c.persons = persons;
            c.departments = departments;
            contracts.Add(c);

            //DO NOT CLOSE THE CONNECTION OR THE TRANSACTION HERE
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
        
        public int AddContract(string title, string start, string end, string typeContractName, string xml, string userName, SortedList persons, int[] destinations, int fileSize, string fileMIMEType, System.IO.BinaryReader fileBinaryReader, byte[] fileBinaryBuffer)
        {
            SqlTransaction transaction = DBUtils.BeginTransaction(IsolationLevel.ReadUncommitted);
            
            int contractFileId = addFile(transaction, fileSize, fileMIMEType, fileBinaryReader, fileBinaryBuffer);

            var parameters = new NameValueCollection
            {
                {"@title", title},
                {"@start", start},
                {"@end", end},
                {"@fileId", contractFileId.ToString()},
                {"@xmlContent", xml},
                {"@userLogin", userName},
                {"@typeContractName", typeContractName},
                {"@archived", "0"}
            };

            int contractId = DBUtils.ExecuteInsert(
                "INSERT INTO [Contract] (title, start, [end], fileId, xmlContent, userLogin, typeContractName, archived) VALUES (@title, @start, @end, @fileId, @xmlContent, @userLogin, @typeContractName, @archived)",
                IsolationLevel.ReadUncommitted, parameters, "Contract", transaction);

            addContacts(transaction, contractId, persons);
            addDestinations(transaction, contractId, destinations);

            transaction.Commit();

            return contractId;
        }


        internal void SaveContract(int id, string title, string start, string end, string typeContractName, string xml, string userName, SortedList persons, int[] destinations, int contractFileId, int fileSize, string fileMIMEType, System.IO.BinaryReader fileBinaryReader, byte[] fileBinaryBuffer)
        {
            SqlTransaction transaction = DBUtils.BeginTransaction(IsolationLevel.ReadUncommitted);

            if (contractFileId != -1)
            {
                updateFile(contractFileId, transaction, fileSize, fileMIMEType, fileBinaryReader, fileBinaryBuffer);
            }

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
                {"@title", title},
                {"@start", start},
                {"@end", end},
                {"@xmlContent", xml},
                {"@userLogin", userName},
                {"@typeContractName", typeContractName},
                {"@archived", "0"}
            };

            DBUtils.ExecuteNonQuery(transaction.Connection,
                "UPDATE [Contract] SET title = @title, start = @start, [end] = @end, xmlContent = @xmlContent, userLogin = @userLogin, typeContractName = @typeContractName, archived = @archived WHERE id = @id",
                transaction, parameters);

            updateContacts(transaction, id, persons);
            updateDestinations(transaction, id, destinations);

            transaction.Commit();
        }

        private int addFile(SqlTransaction transaction, int fileSize, string fileMIMEType, System.IO.BinaryReader fileBinaryReader, byte[] fileBinaryBuffer)
        {

            string fileToDbQueryStr = @"INSERT INTO ContractFile (fileSize, fileMIMEType, fileBinaryData) VALUES(@fileSize, @fileMIMEType, @fileBinaryData)";
            System.Data.SqlClient.SqlCommand command = new SqlCommand(fileToDbQueryStr, transaction.Connection, transaction);

            command.Parameters.AddWithValue("fileSize", fileSize);
            command.Parameters.AddWithValue("fileMIMEType", fileMIMEType);

            SqlParameter imageFileBinaryParam = new SqlParameter("fileBinaryData", SqlDbType.VarBinary, fileSize);
            imageFileBinaryParam.Value = fileBinaryBuffer;

            command.Parameters.Add(imageFileBinaryParam);

            command.ExecuteNonQuery();

            var getCommand = new SqlCommand("SELECT IDENT_CURRENT('ContractFile') AS ID", transaction.Connection, transaction);

            int id;
            using (var reader = getCommand.ExecuteReader())
            {
                reader.Read();

                id = reader["ID"].ToString().ToInt();
            }

            return id;
        }

        private void updateFile(int contractFileId, SqlTransaction transaction, int fileSize, string fileMIMEType, BinaryReader fileBinaryReader, byte[] fileBinaryBuffer)
        {
            string fileToDbQueryStr = @"UPDATE [ContractFile] SET fileSize = @fileSize, fileMIMEType = @fileMIMEType, fileBinaryData = @fileBinaryData WHERE id = @contractFileId";
            System.Data.SqlClient.SqlCommand command = new SqlCommand(fileToDbQueryStr, transaction.Connection, transaction);

            command.Parameters.AddWithValue("contractFileId", contractFileId);
            command.Parameters.AddWithValue("fileSize", fileSize);
            command.Parameters.AddWithValue("fileMIMEType", fileMIMEType);

            SqlParameter imageFileBinaryParam = new SqlParameter("fileBinaryData", SqlDbType.VarBinary, fileSize);
            imageFileBinaryParam.Value = fileBinaryBuffer;

            command.Parameters.Add(imageFileBinaryParam);

            command.ExecuteNonQuery();
        }
     
        private void addContacts(SqlTransaction transaction, int contractId, SortedList persons)
        {
            for (int i = 0; i < persons.Count; i++)
            {
                var parametersContact = new NameValueCollection
                {
                    {"@person", persons.GetKey(i).ToString()},
                    {"@roleName", persons.GetByIndex(i).ToString()},
                    {"@contractId", contractId.ToString()},
                };

               DBUtils.ExecuteNonQuery(transaction.Connection, "INSERT INTO [Association] VALUES (@person, @roleName, @contractId)", transaction, parametersContact);
            }
        }

        private void updateContacts(SqlTransaction transaction, int contractId, SortedList persons)
        {
            var parameters = new NameValueCollection
                {
                    {"@contractId", contractId.ToString()},
                };

            DBUtils.ExecuteNonQuery(transaction.Connection, "DELETE FROM [Association] WHERE contractId = @contractId", transaction, parameters);

            addContacts(transaction, contractId, persons);
        }

        private void updateDestinations(SqlTransaction transaction, int contractId, int[] destinations)
        {
            var parameters = new NameValueCollection
                {
                    {"@contractId", contractId.ToString()},
                };

            DBUtils.ExecuteNonQuery(transaction.Connection, "DELETE FROM [Destination] WHERE contract = @contractId", transaction, parameters);

            addDestinations(transaction, contractId, destinations);
        }

        private void addDestinations(SqlTransaction transaction, int contractId, int[] destinations)
        {
            for (int i = 0; i < destinations.Length; i++)
            {
                var parametersDestination = new NameValueCollection
                {
                    {"@department", destinations[i].ToString()},
                    {"@contractId", contractId.ToString()},
                };

                DBUtils.ExecuteNonQuery(transaction.Connection, "INSERT INTO [Destination] VALUES (@contractId, @department)", transaction, parametersDestination);
            }
        }

        public void ArchiveContract(int id)
        {
            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            DBUtils.ExecuteUpdate(
                "UPDATE [Contract] SET archived = 1 WHERE id = @id",
                IsolationLevel.ReadUncommitted, parameters);
        }

        public List<Contract> SearchContracts(string title, int year, string contractType, int institution, int department, int person, bool archived)
        {
            List<Contract> contracts = new List<Contract>();

            var query = "SELECT C.id, C.title, C.start, C.[end], C.fileId, C.xmlContent, C.userLogin, C.typeContractName, C.archived, D.department, P.institutionId FROM [Contract] C" +
                         " LEFT OUTER JOIN Association A" +
                         " ON A.contractId = C.id"+
                         " LEFT OUTER JOIN Destination D" +
                         " ON D.contract = C.id" +
                         " LEFT OUTER JOIN Department P" +
                         " ON P.id = D.department" +
                         " WHERE C.title LIKE(@title) AND C.typeContractName LIKE(@contractType)";


            if (person != -1)
            {
                query += " AND A.person = @person";
            }
            if (year > 0)
            {
                query += " AND ( YEAR(C.start) = @year OR YEAR(C.[end]) = @year )";
            }
            
            if (department != -1)
            {
                query += " AND D.department = @department";
            }
            else if (institution != -1)
            {
                query += " AND P.institutionId = @institution";
            } 
            if (archived)
            {
                query += " AND archived = 1";
            }

            var parameters = new NameValueCollection
            {
                {"@title", "%" + title + "%"},
                {"@department", department.ToString()},
                {"@institution", institution.ToString()},
                {"@person", person.ToString()},
                {"@contractType", "%"+ contractType + "%"},
                {"@year", year.ToString()},
            };

            using (var reader = DBUtils.ExecuteQuery(query, IsolationLevel.ReadUncommitted, parameters))
            {
                while (reader.Read())
                {
                    contracts.Add(BindContract(reader));
                }
            }

            return contracts;
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

        private static Contract BindContract(SqlResult reader)
        {
            Contract contract = new Contract();

            contract.Id = (int)reader["id"];
            contract.Title = (string)reader["title"];
            contract.Start = (DateTime)reader["start"];//DateTime.ParseExact(reader["start"].ToString(), "yyyy-MM-dd hh:mm:ss", null);
            contract.End = (DateTime)reader["end"];
            contract.XmlContent = (string)reader["xmlContent"];
            contract.User = (string)reader["userLogin"];
            contract.Type = (string)reader["typeContractName"];
            contract.Archived = (bool)reader["archived"];
            contract.fileId = (int)reader["fileId"];

            return contract;
        }

        ///<summary>
        /// Make a search for the historique feature. 
        ///</summary>
        ///<param name="connection">The connection to use</param>
        ///<param name="transaction">The transaction to use</param>
        ///<param name="year">The year to search contracts. </param>
        ///<param name="institutionId">The institution of the contracts. </param>
        ///<param name="departmentId">The department of the contracts. </param>
        ///<returns>All the contracts of the historique. </returns>
        public List<Contract> HistoSearch(SqlConnection connection, SqlTransaction transaction, int year, int institutionId, int departmentId)
        {
            var contracts = new List<Contract>();

            var query = "SELECT DISTINCT C.id, C.title FROM Contract C";

            if(departmentId != -1)
            {
                query += " INNER JOIN Destination D ON D.contract = C.id";
                query += " WHERE D.department = @department";
            }
            else if (institutionId != -1)
            {
                query += " INNER JOIN Destination Dest ON Dest.contract = C.id";
                query += " INNER JOIN Department Dep ON Dep.id = Dest.department";
                query += " WHERE Dep.institutionId = @institution";
            } else
            {
                query += " WHERE 1 = 1";
            }

            query += " AND (YEAR(start) = @year OR YEAR([end]) = @year)";

            Logger.Debug("Histo search with query \"{0}\"", query);

            var command = new SqlCommand(query, connection, transaction);

            command.Parameters.AddWithValue("year", year.ToString());
            command.Parameters.AddWithValue("department", departmentId.ToString());
            command.Parameters.AddWithValue("institution", institutionId.ToString());

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    var contract = new Contract {Id = (int) reader["id"], Title = (string) reader["title"]};

                    contracts.Add(contract);
                }
            }

            Logger.Debug("Histo search found {0} persons", contracts.Count);

            return contracts;
        }
    }
}