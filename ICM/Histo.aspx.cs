using System;
using System.Web.UI;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    public partial class Histo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadLists();
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

                int year = YearTextBox.Text.ToInt();
            }
        }

        protected void InstitutionSelected(object sender, EventArgs e)
        {
            var id = InstitutionList.SelectedValue.ToInt();

            var institution = new InstitutionsDAO().GetInstitution(id);

            if (institution != null)
            {
                DepartmentList.DataBindWithEmptyElement(institution.Departments, "Name", "Id");
            }
        }
    }
}