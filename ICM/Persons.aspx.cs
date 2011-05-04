using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Dao;
using ICM.Utils;
using NLog;

namespace ICM
{
    /// <summary>
    ///  This page enable the users to search for persons in the database. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public partial class Persons : Page
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Load the lists of the page. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if("-1".Equals(IDLabel.Text))
            {
                InstitutionList.DataBindWithEmptyElement(new InstitutionsDAO().GetInstitutions(), "Name", "Id");

                IDLabel.Text = "1";
            }
        }

        /// <summary>
        /// The Search button has been clicked. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void SearchPerson(object sender, EventArgs e)
        {
            SearchPersons();
        }

        /// <summary>
        /// Search for persons using the user criterias and fill the lists with the results. 
        /// </summary>
        private void SearchPersons()
        {
            Logger.Debug("User started search persons");

            var institution = InstitutionList.SelectedValue;
            var department = DepartmentList.SelectedValue;

            var institutionId = "".Equals(institution) ? -1 : institution.ToInt();
            var departmentId = "".Equals(department) ? -1 : department.ToInt();

            try
            {
                var persons = new PersonsDAO().SearchPersons(NameLabel.Text, FirstNameLabel.Text, ArchivedCheckBox.Checked, institutionId, departmentId);

                ErrorLabel.Visible = true;

                ResultsView.DataSource = persons;
                ResultsView.DataBind();
            } 
            catch(SqlException e)
            {
                ErrorLabel.Visible = true;
                ErrorLabel.Text = "Erreur de base de données : " + e.Message;

                Logger.DebugException("SQL Exception during search", e);
            }
        }

        /// <summary>
        /// A person is deleted from the list view. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void PersonDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListViewItem myItem = ResultsView.Items[e.ItemIndex];

            var labelId = myItem.FindControl("LabelID") as Label;

            new PersonsDAO().ArchivePerson(labelId.Text.ToInt());

            SearchPersons();
        }

        /// <summary>
        /// An institution has been selected
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void InstitutionSelected(object sender, EventArgs e)
        {
            var id = InstitutionList.SelectedValue.ToInt();

            var institution = new InstitutionsDAO().GetInstitution(id);

            if (institution != null)
            {
                DepartmentList.DataBindWithEmptyElement(institution.Departments, "Name", "Id");
            }
        }
    }
}