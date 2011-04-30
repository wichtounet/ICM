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
            if (Request.QueryString["person"] != null)
            {
                var id = Request.QueryString["person"].ToInt();

                var person = new PersonsDAO().GetPersonByID(id);

                IDLabel.Text = person.Id.ToString();
                NameLabel.Text = person.Name;
                FirstNameLabel.Text = person.FirstName;
                MailLabel.Text = person.Email;
                PhoneLabel.Text = person.Phone;

                StateLabel.Text = person.Archived ? "Oui" : "Non";
            } 
            else
            {
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
            } 
        }

        protected void EditPerson(object sender, EventArgs e)
        {
            var id = IDLabel.Text.ToInt();

            Response.Redirect("AddPerson.aspx?person=" + id);
        }

        protected void DeletePerson(object sender, EventArgs e)
        {
            var id = IDLabel.Text.ToInt();

            new PersonsDAO().ArchivePerson(id);

            Response.Redirect("ShowPerson.aspx?person=" + id);
        }
    }
}