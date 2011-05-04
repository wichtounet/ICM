using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ICM.Model;
using ICM.Utils;
using ICM.Dao;

namespace ICM.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            List<User> users = new UsersDAO().GetUsers();
            User user = users.GetUserByLogin(UserName.Text);

            if (user == null)   //Wrong login
            {
                FailureLiteral.Text = "Login ou password incorrect";
                return;
            }

            if (!Password.Text.Equals(user.Password))//Wrong password
            {
                FailureLiteral.Text = "Login ou password incorrect";
                return;
            }

            string privilege = user.Admin ? "Admin" : "Guest";
            FormsAuthentication.RedirectFromLoginPage(privilege, RememberCheckBox.Checked);
        }
    }
}
