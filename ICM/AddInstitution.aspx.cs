using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using ICM.Dao;
using ICM.Model;

namespace ICM
{
    public partial class AddInstitution : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            //Languages databound
            LanguagesDAO languagesDAO = new LanguagesDAO();
            List<Language> languages = languagesDAO.GetAllLanguages();
            LanguageList.DataSource = languages;
            LanguageList.DataBind();

            //Continents databound
            CountriesDAO countriesDAO = new CountriesDAO();
            List<Continent> continents = countriesDAO.GetAllContinents();
            ContinentList.DataSource = continents;
            ContinentList.DataBind();

            //Contries databound
            List<Country> countries = countriesDAO.GetCountries(continents[0]);
            CountryList.DataSource = countries;
            CountryList.DataBind();
        }

        //Update CountryList contents
        protected void ContinentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountriesDAO countriesDAO = new CountriesDAO();
            List<Country> countries = countriesDAO.GetCountries(new Continent() { Name = ContinentList.SelectedValue });
            CountryList.DataSource = countries;
            CountryList.DataBind();
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            InstitutionsDAO institutionsDAO = new InstitutionsDAO();

            //Instantiate and fill department list
            List<Department> departments = new List<Department>();
            foreach (ListItem department in DepartmentList.Items)
            {
                departments.Add(new Department() { Name = department.Text });
            }

            //Intantiate institution
            Language language = new Language() {Name = LanguageList.SelectedValue};
            Continent continent = new Continent() {Name = ContinentList.SelectedValue};
            Country country = new Country() {Name = CountryList.SelectedValue, Continent = continent };
            Institution institution = new Institution(  -1,
                                                        NameText.Text,
                                                        DescriptionText.Text,
                                                        CityText.Text,
                                                        InterestText.Text,
                                                        language,
                                                        country,
                                                        departments, 
                                                        false);
            institutionsDAO.AddInstitution(institution);
        }

        protected void AddDepartmentButton_Click(object sender, EventArgs e)
        {
            DepartmentList.Items.Add(new ListItem() {Text=DepartmentText.Text});
            DepartmentText.Text = "";
        }

        protected void RemoveDepartmentButton_Click(object sender, EventArgs e)
        {
            DepartmentList.Items.Remove(DepartmentList.SelectedItem);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            InstitutionsDAO institutionsDAO = new InstitutionsDAO();
            List<Institution> institutions = institutionsDAO.GetInstitutions();

            foreach(Institution i in institutions)
            {
                Response.Write("id: " + i.Id + " name: " + i.Name +"\n");
                foreach (Department d in i.Departments)
                    Response.Write("Department: " + d.Name);
            }
        }

    }
}