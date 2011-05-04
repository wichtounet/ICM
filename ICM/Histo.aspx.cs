using System;
using System.Web.UI;
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
                LoadLists();

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
                Institution institution = null;
                Department department = null;

                if(!"".Equals(InstitutionList.SelectedValue))
                {
                    var institutionId = InstitutionList.SelectedValue.ToInt();

                    institution = new InstitutionsDAO().GetInstitution(institutionId);

                    if(!"".Equals(DepartmentList.SelectedValue))
                    {
                        var departmentId = DepartmentList.SelectedValue.ToInt();

                        //TODO get the good department
                    }
                }

                //TODO Make the search of contracts
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

                var institution = new InstitutionsDAO().GetInstitution(id);

                if (institution != null)
                {
                    DepartmentList.DataBindWithEmptyElement(institution.Departments, "Name", "Id");
                }
            }
        }
    }
}