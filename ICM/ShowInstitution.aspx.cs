using System;
using ICM.Dao;
using ICM.Model;
using ICM.Utils;

namespace ICM
{
    ///<summary>
    /// Enable the user to visualize an institution. 
    ///</summary>
    public partial class ShowInstitution : System.Web.UI.Page
    {
        /// <summary>
        /// Fill the page with the informations of the institution. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                if (Request.QueryString["institution"] != null)
                {
                    var institutionId = Request.QueryString["institution"].ToInt();
                    var institution = new InstitutionsDAO().GetInstitution(institutionId);

                    NameLabel.Text = institution.Name;
                    DescriptionLabel.Text = institution.Description;
                    CityLabel.Text = institution.City;
                    InterestLabel.Text = institution.Interest;
                    LanguageLabel.Text = institution.Language.Name;
                    CountryLabel.Text = institution.Country.Name;
                    ContinentLabel.Text = institution.Country.Continent.Name;

                    if (institution.IsArchived)
                    {
                        StateLabel.Text = "Oui";
                        ArchiveButton.Enabled = false;
                    }
                    else
                    {
                        StateLabel.Text = "Non";
                    }

                    DepartmentsListView.DataSource = institution.Departments;
                    DepartmentsListView.DataBind();

                    ArchiveButton.Enabled = !institution.IsArchived;
                }
                else
                {
                    EditButton.Enabled = false;
                    ArchiveButton.Enabled = false;
                }
            };
            this.Verified(operation, ErrorLabel);
        }

        /// <summary>
        /// Archive the current institution. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void ArchiveButton_Click(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                var institutionId = Request.QueryString["institution"].ToInt();
                new InstitutionsDAO().ArchiveInstitution(institutionId);
                StateLabel.Text = "Oui";
                ArchiveButton.Enabled = false;
            };

            this.Verified(operation, ErrorLabel);
        }

        /// <summary>
        /// Edit the current institution. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            var institutionId = Request.QueryString["institution"].ToInt();

            Response.Redirect("AddInstitution.aspx?institution=" + institutionId);
        }
    }
}