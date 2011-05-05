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
    public partial class Institutions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            //Fill language list
            List<Language> languages = (new LanguagesDAO()).GetAllLanguages();
            languages.Insert(0, new Language() { Name = "" });
            LanguagesList.DataSource = languages;
            LanguagesList.DataBind();

            //Fill continent list
            List<Continent> continents = (new CountriesDAO()).GetAllContinents();
            continents.Insert(0, new Continent() { Name = "" });
            ContinentsList.DataSource = continents;
            ContinentsList.DataBind();
        }

        protected void ContinentsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountriesDAO countriesDAO = new CountriesDAO();
            List<Country> countries = countriesDAO.GetCountries(new Continent() { Name = ContinentsList.SelectedValue });
            countries.Insert(0, new Country() { Name = "" });
            CountriesList.DataSource = countries;
            CountriesList.DataBind();
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            InstitutionsDAO institutionsDAO = new InstitutionsDAO();

            List<Institution> institutions = institutionsDAO.SearchInstitutions(NameText.Text,
                                                LanguagesList.SelectedValue,
                                                ContinentsList.SelectedValue,
                                                CountriesList.SelectedValue,
                                                ArchivedCheckBox.Checked);

            ResultsView.DataSource = institutions;
            ResultsView.DataBind();
        }

        protected void InstitutionArchiving(object sender, ListViewDeleteEventArgs e)
        {
            ListViewItem myItem = ResultsView.Items[e.ItemIndex];
            Label labelId = (Label)myItem.FindControl("LabelID");
            new InstitutionsDAO().ArchiveInstitution(labelId.Text.ToInt());
            //ResultsView.DeleteItem(e.ItemIndex);
            SearchButton_Click(sender, e); //Refresh the page
        }
    }
}