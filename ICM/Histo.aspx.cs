using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    /// <summary>
    ///  This page enable the users to generate an historique. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public partial class Histo : Page
    {
        /// <summary>
        /// Load the lists of the page. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
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
            var dataSource = new InstitutionsDAO().GetInstitutionsClean();

            InstitutionList.DataBindWithEmptyElement(dataSource, "Name", "Id");

            if (dataSource.Count > 0)
            {
                DepartmentList.DataBindWithEmptyElement(dataSource[0].Departments, "Name", "Id");
            }
        }

        /// <summary>
        /// Generate the historique
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
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
                    using(var connection = DBManager.GetInstance().GetNewConnection())
                    {
                        var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                        var contracts = new ContractsDAO().HistoSearch(transaction, year, institutionId, departmentId);

                        ContractsView.DataSource = contracts;
                        ContractsView.DataBind();

                        PersonsView.DataSource = new PersonsDAO().HistoSearch(transaction, contracts);
                        PersonsView.DataBind();

                        transaction.Commit();
                    }
                };

                this.Verified(operation, ErrorLabel);
            }
        }

        /// <summary>
        /// An institution has been selected. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
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
                        DepartmentList.DataBindWithEmptyElement(institution.Departments, "Name", "Id");
                    }
                };

                this.Verified(operation, ErrorLabel);
            }
        }

        /// <summary>
        /// Generate the PDF of the historique
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
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