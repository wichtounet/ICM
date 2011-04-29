using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Model;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    public partial class AddPerson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO Load drop down lists
        }

        protected void CreatePerson(object sender, EventArgs e)
        {
            new PersonsDAO().CreatePerson(FirstNameTextBox.Text, NameTextBox.Text, PhoneTextBox.Text, MailTextBox.Text);
        }
    }
}