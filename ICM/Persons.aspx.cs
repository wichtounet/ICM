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
    public partial class Persons : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SearchPerson(object sender, EventArgs e)
        {
            SearchPersons();
        }

        private void SearchPersons()
        {
            List<Person> persons = new PersonsDAO().SearchPersons(NameLabel.Text, FirstNameLabel.Text);

            ResultsView.DataSource = persons;
            ResultsView.DataBind();
        }

        protected void PersonDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListViewItem myItem = ResultsView.Items[e.ItemIndex];

            var labelId = myItem.FindControl("LabelID") as Label;

            new PersonsDAO().ArchivePerson(labelId.Text.ToInt());

            SearchPersons();
        }
    }
}