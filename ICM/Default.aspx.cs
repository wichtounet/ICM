using System;
using System.Web.UI;
using ICM.Dao;
using System.Collections.Generic;
using ICM.Utils;
using System.Web.UI.WebControls;

namespace ICM
{
    ///<summary>
    /// This page allow the user to make search of contracts. 
    ///</summary>
    public partial class _Default : Page
    {
        /// <summary>
        /// Load the lists of the page. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //In order to not refill the form at postback
            if ("-1".Equals(stateForm.Text))
            {
                stateForm.Text = "1";

                this.Verified(LoadLists, ErrorLabel);
            }
        }

        private void LoadLists()
        {
            using(var connection = DBManager.GetInstance().GetNewConnection())
            {
                InstitutionList.DataBindWithEmptyElement(new InstitutionsDAO().GetInstitutions(connection), "Name", "Id");
                ContractTypeList.DataBindWithEmptyElement(new TypesDAO().GetAllTypes(connection), "Name", "Name");
                PersonneList.DataBindWithEmptyElement(new PersonsDAO().GetAllPersons(connection), "Name", "Id");

                var years = new List<int>();
                for (var i = 2012; i > 2000; i--)
                {
                    years.Add(i);
                }

                YearList.Items.Add(new ListItem(string.Empty, string.Empty));
                YearList.AppendDataBoundItems = true;
                YearList.DataSource = years;
                YearList.DataBind();
            }
        }

        /// <summary>
        /// An institution has been selected
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void InstitutionSelected(object sender, EventArgs e)
        {
            var id = InstitutionList.SelectedValue.ToInt();

            var institution = new InstitutionsDAO().GetInstitutionClean(id);

            if (institution != null)
            {
                DepartmentList.DataBindWithEmptyElement(institution.Departments, "Name", "Id");
            }
        }

        /// <summary>
        /// The Search button has been clicked. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Search_Click(object sender, EventArgs e)
        {
            this.Verified(Search, ErrorLabel);
        }

        /// <summary>
        /// Search for persons using the entered values as criterias.
        /// </summary>
        private void Search()
        {
            var contractType = ContractTypeList.SelectedValue;
            var institution = InstitutionList.SelectedValue;
            var department = DepartmentList.SelectedValue;
            var person = PersonneList.SelectedValue;
            var yearValue = YearList.SelectedValue;

            var institutionId = "".Equals(institution) ? -1 : institution.ToInt();
            var departmentId = "".Equals(department) ? -1 : department.ToInt();
            var personId = "".Equals(person) ? -1 : person.ToInt();
            var year = "".Equals(yearValue) ? -1 : yearValue.ToInt();

            var contracts = new ContractsDAO().SearchContracts(TitleText.Text, year, contractType, institutionId, departmentId, personId, ArchivedCheck.Checked);

            ResultsView.DataSource = contracts;
            ResultsView.DataBind();
        }

        /// <summary>
        /// A contract is deleted from the list view. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void ContractDeleting(object sender, ListViewDeleteEventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                ListViewItem myItem = ResultsView.Items[e.ItemIndex];

                var labelId = myItem.FindControl("LabelID") as Label;

                new ContractsDAO().ArchiveContract(labelId.Text.ToInt());

                Search();
            };
            
            this.Verified(operation, ErrorLabel);
        }
    }
}
