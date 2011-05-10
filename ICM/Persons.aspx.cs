using System;
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
            if ("-1".Equals(IDLabel.Text))
            {
                this.Verified(
                    () => InstitutionList.DataBindWithEmptyElement(new InstitutionsDAO().GetInstitutionsClean(), "Name", "Id"),
                    ErrorLabel);

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
            Logger.Debug("User started search persons");

            this.Verified(SearchPersons, ErrorLabel);
        }

        /// <summary>
        /// Search for persons using the user criterias and fill the lists with the results. 
        /// </summary>
        private void SearchPersons()
        {
            var institutionId = InstitutionList.SelectedValue.ToIntOrDefault();
            var departmentId = DepartmentList.SelectedValue.ToIntOrDefault();

            ResultsView.DataSource = new PersonsDAO().SearchPersons(NameLabel.Text, FirstNameLabel.Text, ArchivedCheckBox.Checked, institutionId, departmentId);
            ResultsView.DataBind();
        }

        /// <summary>
        /// A person is deleted from the list view. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void PersonDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListViewItem myItem = ResultsView.Items[e.ItemIndex];

            var labelId = (Label) myItem.FindControl("LabelID");

            this.Verified(
                () => new PersonsDAO().ArchivePerson(labelId.Text.ToInt()),
                ErrorLabel);

            this.Verified(SearchPersons, ErrorLabel);
        }

        /// <summary>
        /// An institution has been selected
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void InstitutionSelected(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                var id = InstitutionList.SelectedValue.ToIntOrDefault();

                if(id > 0)
                {
                    var institution = new InstitutionsDAO().GetInstitution(id);

                    if (institution != null)
                    {
                        DepartmentList.DataBindWithEmptyElement(institution.Departments, "Name", "Id");
                    }
                }
            };

            this.Verified(operation, ErrorLabel);
        }
    }
}