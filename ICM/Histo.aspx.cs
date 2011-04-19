using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;

namespace ICM
{
    public partial class Histo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*DBManager manager = new DBManager();

            using (SqlConnection connection = manager.getConnection())
            {
                SqlCommand command = new SqlCommand("Select * from [User]", connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    YearTextBox.Text += reader[0].ToString();
                }
            }*/
        }
    }
}