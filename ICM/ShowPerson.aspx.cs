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
    public partial class ShowPerson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["person"].ToInt();

            if(id != null)
            {
                Person person = new PersonsDAO().GetPersonByID(id);

                IDLabel.Text = person.Id.ToString();
                NameLabel.Text = person.Name;
                FirstNameLabel.Text = person.FirstName;
                MailLabel.Text = person.Email;
                PhoneLabel.Text = person.Phone;

                if (person.Archived)
                {
                    StateLabel.Text = "Oui";
                }
                else
                {
                    StateLabel.Text = "Non";
                }
            } 
        }

        protected void EditPerson(object sender, EventArgs e)
        {

        }

        protected void DeletePerson(object sender, EventArgs e)
        {

        }
    }
}