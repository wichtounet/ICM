using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    public partial class Persons : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if("-1".Equals(IDLabel.Text))
            {
                InstitutionList.DataBindWithEmptyElement(new InstitutionsDAO().GetInstitutions(), "Name", "Id");

                IDLabel.Text = "1";
            }
        }

        protected void SearchPerson(object sender, EventArgs e)
        {
            SearchPersons();
        }

        private void SearchPersons()
        {
            var persons = new PersonsDAO().SearchPersons(NameLabel.Text, FirstNameLabel.Text, ArchivedCheckBox.Checked);

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

        protected void InstitutionSelected(object sender, EventArgs e)
        {
            int id = InstitutionList.SelectedValue.ToInt();

            var institution = new InstitutionsDAO().GetInstitution(id);

            if (institution != null)
            {
                DepartmentList.DataBindWithEmptyElement(institution.Departments, "Name", "Id");
            }
        }
    }
}