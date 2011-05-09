using System;
using ICM.Dao;
using System.Collections.Generic;
using ICM.Model;
using ICM.Utils;
using System.Web.UI.WebControls;

namespace ICM
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //In order to not refill the form at postback
            if ("-1".Equals(stateForm.Text))
            {
                stateForm.Text = "1";

                InstitutionList.DataBindWithEmptyElement(new InstitutionsDAO().GetInstitutions(), "Name", "Id");
                ContractTypeList.DataBindWithEmptyElement(new TypesDAO().GetAllTypes(), "Name", "Name");
                PersonneList.DataBindWithEmptyElement(new PersonsDAO().GetAllPersons(), "Name", "Id");

                List<int> years = new List<int>();
                for (int i = 2012; i > 2000; i--)
                {
                    years.Add(i);
                }
                YearList.Items.Add(new ListItem(string.Empty, string.Empty));
                YearList.AppendDataBoundItems = true;
                YearList.DataSource = years;
                YearList.DataBind();

            }
        }

        protected void InstitutionSelected(object sender, EventArgs e)
        {
            int id = InstitutionList.SelectedValue.ToInt();

            var institution = new InstitutionsDAO().GetInstitution(id);

            if (institution != null)
            {
                DepartmentList.DataBindWithEmptyElement(institution.Departments, "Name", "Id");
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            Search();
        }
        private void Search()
        {
            var contractType = ContractTypeList.SelectedValue;
            var institution = InstitutionList.SelectedValue;
            var department = DepartmentList.SelectedValue;
            var person = PersonneList.SelectedValue;
            var yearValue = YearList.SelectedValue;

            int institutionId = "".Equals(institution) ? -1 : institution.ToInt();
            int departmentId = "".Equals(department) ? -1 : department.ToInt();
            int personId = "".Equals(person) ? -1 : person.ToInt();
            int year = "".Equals(yearValue) ? -1 : yearValue.ToInt();

            List<Contract> contracts = new ContractsDAO().SearchContracts(TitleText.Text, year, contractType, institutionId, departmentId, personId, ArchivedCheck.Checked);

            ResultsView.DataSource = contracts;
            ResultsView.DataBind();
        }

        protected void ContractDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListViewItem myItem = ResultsView.Items[e.ItemIndex];

            var labelId = myItem.FindControl("LabelID") as Label;

            new ContractsDAO().ArchiveContract(labelId.Text.ToInt());

            Search();
        }
    }
}
