using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using ICM.Dao;
using ICM.Model;
using ICM.Utils;

namespace ICM
{
    public partial class AddInstitution : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            Extensions.SqlOperation operation = () =>
            {
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

                //Add institution
                if (Request.QueryString["institution"] == null)
                {
                    EditButton.Visible = false;
                    AddButton.Visible = true;

                    //Contries databound
                    List<Country> countries = countriesDAO.GetCountries(continents[0]);
                    CountryList.DataSource = countries;
                    CountryList.DataBind();
                }
                //Edit institution
                else
                {
                    //TODO: start transaction to commit add button click
                    EditButton.Visible = true;
                    AddButton.Visible = false;

                    int institutionId = Request.QueryString["institution"].ToInt();
                    Institution institution = new InstitutionsDAO().GetInstitution(institutionId);


                    /*
                    var connection = DBManager.GetInstance().GetNewConnection();

                    var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                    new PersonsDAO().LockPerson(id, transaction, connection);

                    var tr = Interlocked.Increment(ref transactionId);
                
                    Session["connection" + tr] = connection;
                    Session["transaction" + tr] = transaction;

                    ViewState["transaction"] = tr;
                    */

                    NameText.Text = institution.Name;
                    DescriptionText.Text = institution.Description;
                    CityText.Text = institution.City;
                    ContinentList.SelectedValue = institution.Country.Continent.Name;

                    //Contries databound
                    List<Country> countries = countriesDAO.GetCountries(institution.Country.Continent);
                    CountryList.DataSource = countries;
                    CountryList.DataBind();

                    LanguageList.SelectedValue = institution.Language.Name;
                    InterestText.Text = institution.Interest;

                    //Departments databound
                    DepartmentList.DataSource = institution.Departments;
                    DepartmentList.DataBind();
                }
            };

            this.Verified(operation, ErrorLabel);
        }

        //Update CountryList contents
        protected void ContinentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                CountriesDAO countriesDAO = new CountriesDAO();
                List<Country> countries = countriesDAO.GetCountries(new Continent() { Name = ContinentList.SelectedValue });
                CountryList.DataSource = countries;
                CountryList.DataBind();
            };
            this.Verified(operation, ErrorLabel);
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                InstitutionsDAO institutionsDAO = new InstitutionsDAO();

                //Instantiate and fill department list
                List<Department> departments = new List<Department>();
                foreach (ListItem department in DepartmentList.Items)
                {
                    departments.Add(new Department() { Name = department.Text });
                }

                //Intantiate institution
                Language language = new Language() { Name = LanguageList.SelectedValue };
                Continent continent = new Continent() { Name = ContinentList.SelectedValue };
                Country country = new Country() { Name = CountryList.SelectedValue, Continent = continent };
                Institution institution = new Institution(-1,
                                                            NameText.Text,
                                                            DescriptionText.Text,
                                                            CityText.Text,
                                                            InterestText.Text,
                                                            language,
                                                            country,
                                                            departments,
                                                            false);
                institutionsDAO.AddInstitution(institution);
            };
            this.Verified(operation, ErrorLabel);
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

        protected void EditButton_Click(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                int institutionId = Request.QueryString["institution"].ToInt();
                InstitutionsDAO institutionsDAO = new InstitutionsDAO();

                //Instantiate and fill department list
                List<Department> departments = new List<Department>();
                foreach (ListItem department in DepartmentList.Items)
                {
                    departments.Add(new Department() { Name = department.Text });
                }

                //Intantiate institution
                Language language = new Language() { Name = LanguageList.SelectedValue };
                Continent continent = new Continent() { Name = ContinentList.SelectedValue };
                Country country = new Country() { Name = CountryList.SelectedValue, Continent = continent };
                Institution institution = new Institution(institutionId,
                                                            NameText.Text,
                                                            DescriptionText.Text,
                                                            CityText.Text,
                                                            InterestText.Text,
                                                            language,
                                                            country,
                                                            departments,
                                                            false);
                institutionsDAO.UpdateInstitution(institution);
            };
            this.Verified(operation, ErrorLabel);
        }
    }
}