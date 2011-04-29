using System;
using System.Web.UI;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    public partial class ShowPerson : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["person"].ToInt();

            if(id != null)
            {
                var person = new PersonsDAO().GetPersonByID(id);

                IDLabel.Text = person.Id.ToString();
                NameLabel.Text = person.Name;
                FirstNameLabel.Text = person.FirstName;
                MailLabel.Text = person.Email;
                PhoneLabel.Text = person.Phone;

                StateLabel.Text = person.Archived ? "Oui" : "Non";
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