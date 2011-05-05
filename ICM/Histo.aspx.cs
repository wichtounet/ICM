using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Dao;
using ICM.Model;
using ICM.Utils;

namespace ICM
{
    public partial class Histo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //In order to not refill the form at postback
            if ("-1".Equals(IDLabel.Text))
            {
                this.Verified(LoadLists, ErrorLabel);

                IDLabel.Text = "1";
            }
        }

        private void LoadLists()
        {
            var dataSource = new InstitutionsDAO().GetInstitutions();

            InstitutionList.DataBindWithEmptyElement(dataSource, "Name", "Id");

            if (dataSource.Count > 0)
            {
                DepartmentList.DataBindWithEmptyElement(dataSource[0].Departments, "Name", "Id");
            }
        }

        protected void SearchHisto(object sender, EventArgs e)
        {
            if(Page.IsValid)
            {
                HistoPanel.Visible = true;

                var year = YearTextBox.Text.ToInt();
                var institutionId = "".Equals(InstitutionList.SelectedValue) ? -1 : InstitutionList.SelectedValue.ToInt();
                var departmentId = "".Equals(DepartmentList.SelectedValue) ? -1 : DepartmentList.SelectedValue.ToInt();

                Extensions.SqlOperation operation = () =>
                {
                    var contracts = new ContractsDAO().HistoSearch(year, institutionId, departmentId);

                    ContractsView.DataSource = contracts;
                    ContractsView.DataBind();

                    var persons = new PersonsDAO().HistoSearch(contracts);

                    PersonsView.DataSource = persons;
                    PersonsView.DataBind();
                };

                this.Verified(operation, ErrorLabel);
            }
        }

        protected void InstitutionSelected(object sender, EventArgs e)
        {
            if("".Equals(InstitutionList.SelectedValue))
            {
                DepartmentList.Items.Clear();
            } 
            else
            {
                var id = InstitutionList.SelectedValue.ToInt();

                Extensions.SqlOperation operation = () =>
                {
                    var institution = new InstitutionsDAO().GetInstitution(id);

                    if (institution != null)
                    {
                        DepartmentList.DataBindWithEmptyElement(
                            institution.Departments, "Name", "Id");
                    }
                };

                this.Verified(operation, ErrorLabel);
            }
        }

        protected void GeneratePDF(object sender, EventArgs e)
        {
            var contracts = ContractsView.Items;
            var persons = PersonsView.Items;

            var contractList = contracts.Aggregate("", (current, contract) => current + (((Label) contract.FindControl("LabelID")).Text.ToInt() + ";"));
            var personList = persons.Aggregate("", (current, person) => current + (((Label) person.FindControl("LabelID")).Text.ToInt() + ";"));

            Response.Redirect("HistoPDF.aspx?persons=" + personList + "&contracts=" + contractList + "&year=" + YearTextBox.Text);
        }
    }
}