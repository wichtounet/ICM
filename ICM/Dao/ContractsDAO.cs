using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ICM.Model;
using ICM.Utils;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.SqlServer.Server;
using NLog;
using System.Xml;

namespace ICM.Dao
{
    /// <summary>
    ///  This class enables the user to make operations on the "Contract" table and all join table. With this DAO, you can create, update, delete and search for contracts. 
    /// </summary>
    /// <remarks>Vincent Ischi</remarks>
    public class ContractsDAO
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get a contract by an ID
        /// </summary>
        /// <param name="id">ID of contract</param>
        /// <returns>The contract with this ID</returns>
        public Contract GetContractById(int id)
        {
            using(var connection = DBManager.GetInstance().GetNewConnection())
            {
                var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                var person = GetContractById(id, transaction);

                transaction.Commit();

                return person;
            }
        }

        /// <summary>
        /// Get a contract by an ID with a specify transaction already opened
        /// </summary>
        /// <param name="id">ID of contract</param>
        /// <param name="transaction">The transaction to use</param>
        /// <returns>The contract with this ID</returns>
        public Contract GetContractById(int id, SqlTransaction transaction)
        {
            var contracts = new List<Contract>();

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            var c = new Contract();
            var persons = new List<Person>();
            var departments = new List<Department>();

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
                    var p = new Person
                    {
                        Id = (int) readerAssoc["personId"],
                        Role = (string) readerAssoc["roleName"],
                        FirstName = (string) readerAssoc["firstname"],
                        Name = (string) readerAssoc["name"]
                    };

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
                    var d = new Department
                    {
                        Id = (int) readerDestination["departementId"],
                        Name = (string) readerDestination["depName"],
                        InstitutionName = (string) readerDestination["insName"],
                        InstitutionId = (int) readerDestination["institutionId"]
                    };

                    departments.Add(d);
                }
            }

            c.persons = persons;
            c.departments = departments;
            contracts.Add(c);

            return contracts.First();
        }

        /// <summary>
        /// Get a contract file by an ID and transform the context.response
        /// </summary>
        /// <param name="context">Context HTTP of page</param>
        /// <param name="id">ID of contract</param>
        public void GetContractFile(HttpContext context, int id)
        {
            try
            {
                using(var connection = DBManager.GetInstance().GetNewConnection())
                {
                    var parameters = new NameValueCollection
                    {
                        {"@id", id.ToString()}
                    };

                    using (var reader = DBUtils.ExecuteQuery("SELECT fileMIMEType,fileBinaryData FROM [ContractFile] WHERE id = @id", connection, IsolationLevel.ReadUncommitted, parameters))
                    {
                        if (reader.Read())
                        {
                            var mimeType = ((string) reader["fileMIMEType"]);
                            var extension = mimeType.Substring(mimeType.LastIndexOf("/") + 1);

                            context.Response.ClearHeaders();
                            context.Response.ClearContent();
                            context.Response.AppendHeader("Pragma", "no-cache");
                            context.Response.AppendHeader("Cache-Control", "no-cache");
                            context.Response.AppendHeader("content-disposition", "attachment; filename=" + "attach." + extension);
                            context.Response.CacheControl = "no-cache";
                            context.Response.Expires = -1;
                            context.Response.ExpiresAbsolute = new DateTime(1900, 1, 1);
                            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            context.Response.ContentType = mimeType;
                            context.Response.BinaryWrite((byte[])reader["fileBinaryData"]);
                            context.Response.Flush();
                            context.Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.ToString());
            } 
        }

        /// <summary>
        /// Get a contract XML by an ID (used by xslt)
        /// </summary>
        /// <param name="id">ID of contract</param>
        /// <returns>a new XMLDocument contains all contract information</returns>
        public XmlDocument getContractXMLById(int id)
        {
            Logger.Debug("Get Contract XML of {0}", id.ToString());

            var xmlDoc = new XmlDocument();

            using (var connection = DBManager.GetInstance().GetNewConnection())
            {
                var parameters = new NameValueCollection
                    {
                        {"@id", id.ToString()}
                    };

                using (var reader = DBUtils.ExecuteQuery("SELECT xmlContent FROM [Contract] WHERE id = @id", connection, IsolationLevel.ReadUncommitted, parameters))
                {
                    if (reader.Read())
                    {
                        xmlDoc.LoadXml((string)reader["XmlContent"]);
                    }
                }
            }

            return xmlDoc;
        }

        /// <summary>
        /// Add a new contract in 'Contract' table, with all dependance (destination, association, file).
        /// </summary>
        /// <param name="title">Title of contract</param>
        /// <param name="start">Start date of contract</param>
        /// <param name="end">End date of contract</param>
        /// <param name="typeContractName">Type of contract</param>
        /// <param name="xml">XML of contract</param>
        /// <param name="userName">UserName who create the contract</param>
        /// <param name="persons">List of contacts of contract</param>
        /// <param name="destinations">List of destination (department) of contract</param>
        /// <param name="fileSize">Size of file upload of contract</param>
        /// <param name="fileMIMEType">Type of file upload of contract</param>
        /// <param name="fileBinaryReader">Binary reader of file upload of contract</param>
        /// <param name="fileBinaryBuffer">Buffer of file upload of contract</param>
        /// <returns>new contract ID</returns>
        public int AddContract(string title, string start, string end, string typeContractName, string xml, string userName, SortedList persons, int[] destinations, int fileSize, string fileMIMEType, System.IO.BinaryReader fileBinaryReader, byte[] fileBinaryBuffer)
        {
            var associationList = GetAssociationList(persons);
            var destinationList = GetDestinationList(destinations);

            using(var connection = DBManager.GetInstance().GetNewConnection())
            {
                var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                try
                {
                    var contractFileId = AddFile(transaction, fileSize, fileMIMEType, fileBinaryBuffer);

                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.insert_contract";

                    command.Parameters.Add("@associations", SqlDbType.Structured);
                    command.Parameters["@associations"].Direction = ParameterDirection.Input;
                    command.Parameters["@associations"].TypeName = "association_list";
                    command.Parameters["@associations"].Value = associationList;

                    command.Parameters.Add("@destinations", SqlDbType.Structured);
                    command.Parameters["@destinations"].Direction = ParameterDirection.Input;
                    command.Parameters["@destinations"].TypeName = "destination_list";
                    command.Parameters["@destinations"].Value = destinationList;

                    command.Parameters.Add(new SqlParameter("@contractId", 0) {Direction = ParameterDirection.Output});

                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@start", start);
                    command.Parameters.AddWithValue("@end", end);
                    command.Parameters.AddWithValue("@fileId", contractFileId.ToString());
                    command.Parameters.AddWithValue("@xmlContent", xml);
                    command.Parameters.AddWithValue("@userLogin", userName);
                    command.Parameters.AddWithValue("@typeContractName", typeContractName);

                    command.ExecuteNonQuery();
                    
                    transaction.Commit();

                    return Int32.Parse(command.Parameters["@contractId"].Value.ToString());
                } 
                catch (SqlException e)
                {
                    transaction.Rollback();

                    throw e;
                }
            }
        }

        private static List<SqlDataRecord> GetDestinationList(int[] destinations)
        {
            var destinationList = new List<SqlDataRecord>();
            SqlMetaData[] destinationDefinition = { new SqlMetaData("department", SqlDbType.Int) };

            foreach (var destination in destinations)
            {
                var record = new SqlDataRecord(destinationDefinition);
                record.SetInt32(0, destination);
                destinationList.Add(record);
            }
            return destinationList;
        }

        private static List<SqlDataRecord> GetAssociationList(SortedList persons)
        {
            var associationList = new List<SqlDataRecord>();
            SqlMetaData[] associationDefinition = { new SqlMetaData("person", SqlDbType.Int), new SqlMetaData("role", SqlDbType.VarChar, 50) };

            for (var i = 0; i < persons.Count; i++)
            {
                var record = new SqlDataRecord(associationDefinition);
                record.SetInt32(0, persons.GetKey(i).ToString().ToInt());
                record.SetString(1, persons.GetByIndex(i).ToString());
                associationList.Add(record);
            }
            return associationList;
        }

        /// <summary>
        /// Add a file in 'ContractFile' table
        /// </summary>
        /// <param name="transaction">Transaction opened</param>
        /// <param name="fileSize">Size of file upload of contract</param>
        /// <param name="fileMIMEType">Type of file upload of contract</param>
        /// <param name="fileBinaryBuffer">Buffer of file upload of contract</param>
        /// <returns>ID of new file</returns>
        private static int AddFile(SqlTransaction transaction, int fileSize, string fileMIMEType, byte[] fileBinaryBuffer)
        {
            var fileToDbQueryStr = @"INSERT INTO ContractFile (fileSize, fileMIMEType, fileBinaryData) VALUES(@fileSize, @fileMIMEType, @fileBinaryData)";
            var command = new SqlCommand(fileToDbQueryStr, transaction.Connection, transaction);

            command.Parameters.AddWithValue("fileSize", fileSize);
            command.Parameters.AddWithValue("fileMIMEType", fileMIMEType);

            var imageFileBinaryParam = new SqlParameter("fileBinaryData", SqlDbType.VarBinary, fileSize);
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


        /// <summary>
        /// Save a contract
        /// </summary>
        /// <param name="transaction">Transaction opened</param>
        /// <param name="id">id of contract</param>
        /// <param name="title">Title of contract</param>
        /// <param name="start">Start date of contract</param>
        /// <param name="end">End date of contract</param>
        /// <param name="typeContractName">Type of contract</param>
        /// <param name="xml">XML of contract</param>
        /// <param name="userName">UserName who create the contract</param>
        /// <param name="persons">List of contacts of contract</param>
        /// <param name="destinations">List of destination (department) of contract</param>
        /// <param name="contractFileId">The id of the contract file</param>
        /// <param name="fileSize">Size of file upload of contract</param>
        /// <param name="fileMIMEType">Type of file upload of contract</param>
        /// <param name="fileBinaryBuffer">Buffer of file upload of contract</param>
        public void SaveContract(SqlTransaction transaction, int id, string title, string start, string end, string typeContractName, string xml, string userName, SortedList persons, int[] destinations, int contractFileId, int fileSize, string fileMIMEType, byte[] fileBinaryBuffer)
        {
            var associationList = GetAssociationList(persons);
            var destinationList = GetDestinationList(destinations);

            try
            {
                UpdateFile(contractFileId, transaction, fileSize, fileMIMEType, fileBinaryBuffer);

                var command = transaction.Connection.CreateCommand();
                command.Transaction = transaction;

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.update_contract";

                command.Parameters.Add("@associations", SqlDbType.Structured);
                command.Parameters["@associations"].Direction = ParameterDirection.Input;
                command.Parameters["@associations"].TypeName = "association_list";
                command.Parameters["@associations"].Value = associationList;

                command.Parameters.Add("@destinations", SqlDbType.Structured);
                command.Parameters["@destinations"].Direction = ParameterDirection.Input;
                command.Parameters["@destinations"].TypeName = "destination_list";
                command.Parameters["@destinations"].Value = destinationList;

                command.Parameters.AddWithValue("@contractId", id);
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@start", start);
                command.Parameters.AddWithValue("@end", end);
                command.Parameters.AddWithValue("@fileId", contractFileId.ToString());
                command.Parameters.AddWithValue("@xmlContent", xml);
                command.Parameters.AddWithValue("@userLogin", userName);
                command.Parameters.AddWithValue("@typeContractName", typeContractName);

                command.ExecuteNonQuery();
            } catch (SqlException e)
            {
                transaction.Rollback();

                throw e;
            }
        }

        /// <summary>
        /// Update a file in 'ContractFile' table
        /// </summary>
        /// <param name="contractFileId">id of file in table</param>
        /// <param name="transaction">Transaction opened</param>
        /// <param name="fileSize">Size of file upload of contract</param>
        /// <param name="fileMIMEType">Type of file upload of contract</param>
        /// <param name="fileBinaryBuffer">Buffer of file upload of contract</param>
        private static void UpdateFile(int contractFileId, SqlTransaction transaction, int fileSize, string fileMIMEType, byte[] fileBinaryBuffer)
        {
            var fileToDbQueryStr = @"UPDATE [ContractFile] SET fileSize = @fileSize, fileMIMEType = @fileMIMEType, fileBinaryData = @fileBinaryData WHERE id = @contractFileId";
            var command = new SqlCommand(fileToDbQueryStr, transaction.Connection, transaction);

            command.Parameters.AddWithValue("contractFileId", contractFileId);
            command.Parameters.AddWithValue("fileSize", fileSize);
            command.Parameters.AddWithValue("fileMIMEType", fileMIMEType);

            var imageFileBinaryParam = new SqlParameter("fileBinaryData", SqlDbType.VarBinary, fileSize);
            imageFileBinaryParam.Value = fileBinaryBuffer;

            command.Parameters.Add(imageFileBinaryParam);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Archive a contract
        /// </summary>
        /// <param name="id">ID of contract</param>
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

        /// <summary>
        /// Search a list of contracts with criteria
        /// </summary>
        /// <param name="title">Title of contract</param>
        /// <param name="year">Year of contract</param>
        /// <param name="contractType">Type of contract</param>
        /// <param name="institution">A institution of contract</param>
        /// <param name="department">A department of contract</param>
        /// <param name="person">A Person of contract</param>
        /// <param name="archived">Is the contract archived</param>
        /// <returns>The list of contracts </returns>
        public List<Contract> SearchContracts(string title, int year, string contractType, int institution, int department, int person, bool archived)
        {
            var contracts = new List<Contract>();

            var query = "SELECT DISTINCT C.id, C.title FROM [Contract] C"+
                         " WHERE C.title LIKE(@title) AND C.typeContractName LIKE(@contractType)";


            if (person != -1)
            {
                query += " AND C.id IN (SELECT DISTINCT contractId FROM [Association] WHERE person = @person AND contractId = C.id)";
            }
            if (year > 0)
            {
                query += " AND ( YEAR(C.start) = @year OR YEAR(C.[end]) = @year )";
            }
            
            if (department != -1)
            {
                query += " AND C.id IN (SELECT DISTINCT contract FROM [Destination] WHERE department =  @department AND contract = C.id)";
            }
            else if (institution != -1)
            {
                query += " AND C.id IN (SELECT DISTINCT contract FROM [Destination] WHERE department IN (SELECT DISTINCT id FROM [department] WHERE institutionId = @institution)) ";
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

            using (var connection = DBManager.GetInstance().GetNewConnection())
            {
                using (var reader = DBUtils.ExecuteQuery(query, connection, IsolationLevel.ReadUncommitted, parameters))
                {
                    while (reader.Read())
                    {
                        Contract c = new Contract();
                        c.Id = (int)reader["id"];
                        c.Title = (string)reader["title"];
                        contracts.Add(c);
                    }
                }
            }
            
            return contracts;
        }

        /// <summary>
        /// Bind a SQLResult to a Contract
        /// </summary>
        /// <param name="reader">Result of SQL request</param>
        /// <returns>A new contract </returns>
        private static Contract BindContract(SqlResult reader)
        {
            return new Contract
            {
                Id = (int) reader["id"],
                Title = (string) reader["title"],
                Start = (DateTime) reader["start"],
                End = (DateTime) reader["end"],
                XmlContent = (string) reader["xmlContent"],
                User = (string) reader["userLogin"],
                Type = (string) reader["typeContractName"],
                Archived = (bool) reader["archived"],
                fileId = (int) reader["fileId"]
            };
        }

        ///<summary>
        /// Make a search for the historique feature. 
        ///</summary>
        ///<param name="transaction">The transaction to use</param>
        ///<param name="year">The year to search contracts. </param>
        ///<param name="institutionId">The institution of the contracts. </param>
        ///<param name="departmentId">The department of the contracts. </param>
        ///<returns>All the contracts of the historique. </returns>
        public List<Contract> HistoSearch(SqlTransaction transaction, int year, int institutionId, int departmentId)
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

            var command = new SqlCommand(query, transaction.Connection, transaction);

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

        ///<summary>
        /// Lock a contract to edition
        ///</summary>
        ///<param name="id">ID of contract</param>
        ///<param name="transaction">The transaction to use</param>
        public void LockContract(int id, SqlTransaction transaction)
        {
            Logger.Debug("Lock Contract with ID ({0})", id);

            var parameters = new NameValueCollection
            {
                {"@id", id.ToString()},
            };

            var command = new SqlCommand("UPDATE Contract set title = title WHERE id = @id", transaction.Connection, transaction) { CommandTimeout = 3 };

            foreach (var key in parameters.AllKeys)
            {
                command.Parameters.AddWithValue(key, parameters.Get(key));
            }

            command.ExecuteNonQuery();
        }
    }
}