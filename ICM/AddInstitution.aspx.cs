using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using ICM.Model;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    public partial class AddInstitution : Page
    {
        private static int transactionId;

        protected void Page_Load(object sender, EventArgs e)
        {
            DepartmentLabel.Visible = false;
            if (IsPostBack)
                return;
            Extensions.SqlOperation operation = () =>
            {
                //Languages databound
                var languagesDAO = new LanguagesDAO();
                var languages = languagesDAO.GetAllLanguages();
                LanguageList.DataSource = languages;
                LanguageList.DataBind();

                //Continents databound
                var countriesDAO = new CountriesDAO();
                var continents = countriesDAO.GetAllContinents();
                ContinentList.DataSource = continents;
                ContinentList.DataBind();

                //Add institution
                if (Request.QueryString["institution"] == null)
                {
                    EditButton.Visible = false;
                    AddButton.Visible = true;

                    //Contries databound
                    var countries = countriesDAO.GetCountries(continents[0]);
                    CountryList.DataSource = countries;
                    CountryList.DataBind();
                }
                //Edit institution
                else
                {
                    var institutionId = Request.QueryString["institution"].ToInt();

                    var connection = DBManager.GetInstance().GetNewConnection();
                    var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                    new InstitutionsDAO().LockInstitution(institutionId, transaction);

                    var tr = Interlocked.Increment(ref transactionId);

                    Session["connection" + tr] = connection;
                    Session["transaction" + tr] = transaction;

                    ViewState["transaction"] = tr;

                    //TODO: start transaction to commit add button click
                    EditButton.Visible = true;
                    AddButton.Visible = false;

                    var institution = new InstitutionsDAO().GetInstitution(institutionId, transaction);

                    NameText.Text = institution.Name;
                    DescriptionText.Text = institution.Description;
                    CityText.Text = institution.City;
                    ContinentList.SelectedValue = institution.Country.Continent.Name;

                    //Contries databound
                    var countries = countriesDAO.GetCountries(institution.Country.Continent);
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
                var countriesDAO = new CountriesDAO();
                var countries = countriesDAO.GetCountries(new Continent { Name = ContinentList.SelectedValue });
                CountryList.DataSource = countries;
                CountryList.DataBind();
            };
            this.Verified(operation, ErrorLabel);
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                var institutionsDAO = new InstitutionsDAO();

                var departments = (from ListItem department in DepartmentList.Items select new Department {Name = department.Text}).ToList();

                //Intantiate institution
                var language = new Language { Name = LanguageList.SelectedValue };
                var continent = new Continent { Name = ContinentList.SelectedValue };
                var country = new Country { Name = CountryList.SelectedValue, Continent = continent };
                var institution = new Institution(-1,
                                                            NameText.Text,
                                                            DescriptionText.Text,
                                                            CityText.Text,
                                                            InterestText.Text,
                                                            language,
                                                            country,
                                                            departments,
                                                            false);
                var institutionId = institutionsDAO.AddInstitution(institution);
                Response.Redirect("ShowInstitution.aspx?institution=" + institutionId);
            };
            this.Verified(operation, ErrorLabel);
        }

        protected void AddDepartmentButton_Click(object sender, EventArgs e)
        {
            if (DepartmentText.Text.Equals(""))
            {
                DepartmentLabel.Visible = true;
                return;
            }
            DepartmentList.Items.Add(new ListItem {Text=DepartmentText.Text});
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
                var tr = (int) ViewState["transaction"];
                var transaction = (SqlTransaction) Session["transaction" + tr];
                var connection = (SqlConnection) Session["connection" + tr];


                var institutionId = Request.QueryString["institution"].ToInt();
                var institutionsDAO = new InstitutionsDAO();

                //Instantiate and fill department list
                var departments = (from ListItem department in DepartmentList.Items select new Department {Name = department.Text}).ToList();

                //Intantiate institution
                var language = new Language { Name = LanguageList.SelectedValue };
                var continent = new Continent { Name = ContinentList.SelectedValue };
                var country = new Country { Name = CountryList.SelectedValue, Continent = continent };
                var institution = new Institution(institutionId,
                                                            NameText.Text,
                                                            DescriptionText.Text,
                                                            CityText.Text,
                                                            InterestText.Text,
                                                            language,
                                                            country,
                                                            departments,
                                                            false);

                institutionsDAO.UpdateInstitution(institution, transaction);

                transaction.Commit();
                connection.Close();

                Response.Redirect("ShowInstitution.aspx?institution=" + institutionId);
            };
            this.Verified(operation, ErrorLabel);
        }
    }
}