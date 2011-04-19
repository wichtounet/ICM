﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;

namespace ICM
{
    public class DBManager
    {
        private SqlConnection connection;

        public SqlConnection getConnection()
        {
            if (connection == null)
            {
                connection = new SqlConnection(@"Data Source=160.98.60.35\MSSQLSERVER,1433;Initial Catalog=ICM;Integrated Security=False;User ID=sa;Password=International3");
                
                connection.Open();
            }

            return connection;
        }

        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}