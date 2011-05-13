using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ICM.Dao;
using ICM.Model;
using ICM.Utils;

namespace ICM
{
    ///<summary>
    /// Enable the user to search for institutions. 
    ///</summary>
    public partial class Institutions : System.Web.UI.Page
    {
        /// <summary>
        /// Load the lists
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            Extensions.SqlOperation operation = () =>
            {
                //Fill language list
                var languages = new LanguagesDAO().GetAllLanguages();
                LanguagesList.DataBindWithEmptyElement(languages, "Name", "Name");

                //Fill continent list
                var continents = new CountriesDAO().GetAllContinents();
                ContinentsList.DataBindWithEmptyElement(continents, "Name", "Name");
            };

            this.Verified(operation, ErrorLabel);
        }

        /// <summary>
        /// A continent has been selected
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void ContinentsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                var countries = new CountriesDAO().GetCountries(new Continent { Name = ContinentsList.SelectedValue });
                CountriesList.DataBindWithEmptyElement(countries, "Name", "Name");
            };

            this.Verified(operation, ErrorLabel);
        }

        /// <summary>
        /// Search for institutions. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                var institutionsDAO = new InstitutionsDAO();

                var institutions = institutionsDAO.SearchInstitutions(NameText.Text,
                                                    LanguagesList.SelectedValue,
                                                    ContinentsList.SelectedValue,
                                                    CountriesList.SelectedValue,
                                                    ArchivedCheckBox.Checked);

                ResultsView.DataSource = institutions;
                ResultsView.DataBind();
            };

            this.Verified(operation, ErrorLabel);
        }

        /// <summary>
        /// Archive the selected institution. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void InstitutionArchiving(object sender, ListViewDeleteEventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                ListViewItem myItem = ResultsView.Items[e.ItemIndex];
                var labelId = (Label)myItem.FindControl("LabelID");
                new InstitutionsDAO().ArchiveInstitution(labelId.Text.ToInt());

                SearchButton_Click(sender, e); //Refresh the page
            };

            this.Verified(operation, ErrorLabel);
        }
    }
}