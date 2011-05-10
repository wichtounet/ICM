using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Dao;
using ICM.Model;
using ICM.Utils;

namespace ICM
{
    public partial class ShowInstitution : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                if (Request.QueryString["institution"] != null)
                {
                    int institutionId = Request.QueryString["institution"].ToInt();
                    Institution institution = new InstitutionsDAO().GetInstitution(institutionId);

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

        protected void ArchiveButton_Click(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                int institutionId = Request.QueryString["institution"].ToInt();
                new InstitutionsDAO().ArchiveInstitution(institutionId);
                StateLabel.Text = "Oui";
                ArchiveButton.Enabled = false;
            };
            this.Verified(operation, ErrorLabel);
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            int institutionId = Request.QueryString["institution"].ToInt();
            Response.Redirect("AddInstitution.aspx?institution=" + institutionId);
        }
    }
}