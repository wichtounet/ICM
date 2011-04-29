using System;
using ICM.Dao;

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