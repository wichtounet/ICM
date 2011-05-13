using System;
using System.Web.Security;
using ICM.Utils;
using ICM.Dao;

namespace ICM.Account
{
    ///<summary>
    /// Enable the user to log into the page. 
    ///</summary>
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Nothing to do here
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                var users = new UsersDAO().GetUsers();
                var user = users.GetUserByLogin(UserName.Text);

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

                Session["userLogin"] = user.Login;

                FormsAuthentication.RedirectFromLoginPage("User", RememberCheckBox.Checked);
            };
            this.Verified(operation, ErrorLabel);
        }
    }
}
