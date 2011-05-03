using System;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    public partial class AddPerson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["person"] != null)
            {
                //In order to not refill the form at postback
                if("-1".Equals(IDLabel.Text) )
                {
                    var id = Request.QueryString["person"].ToInt();

                    var person = new PersonsDAO().GetPersonByID(id);

                    IDLabel.Text = person.Id.ToString();
                    NameTextBox.Text = person.Name;
                    FirstNameTextBox.Text = person.FirstName;
                    MailTextBox.Text = person.Email;
                    PhoneTextBox.Text = person.Phone;

                    SaveButton.Visible = true;

                    LoadLists();
                }
            } 
            else
            {
                AddButton.Visible = true;

                LoadLists();
            }
        }

        private void LoadLists()
        {
            var institutionsDao = new InstitutionsDAO();

            var dataSource = institutionsDao.GetInstitutions();

            InstitutionList.DataSource = dataSource;
            InstitutionList.DataValueField = "Id";
            InstitutionList.DataTextField = "Name";
            InstitutionList.DataBind();
            
            if(dataSource.Count > 0)
            {
                DepartmentList.DataSource = dataSource[0].Departments;
                DepartmentList.DataValueField = "Id";
                DepartmentList.DataTextField = "Name";
                DepartmentList.DataBind();
            }
        }

        protected void CreatePerson(object sender, EventArgs e)
        {
            if(Page.IsValid)
            {
                var id = new PersonsDAO().CreatePerson(FirstNameTextBox.Text, NameTextBox.Text, PhoneTextBox.Text, MailTextBox.Text);

                Response.Redirect("ShowPerson.aspx?person=" + id);
            }
        }

        protected void SavePerson(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var id = IDLabel.Text.ToInt();

                new PersonsDAO().SavePerson(id, FirstNameTextBox.Text, NameTextBox.Text, PhoneTextBox.Text, MailTextBox.Text);

                Response.Redirect("ShowPerson.aspx?person=" + id);
            }
        }

        protected void InstitutionSelected(object sender, EventArgs e)
        {
            var id = InstitutionList.SelectedValue.ToInt();

            var institution = new InstitutionsDAO().GetInstitution(id);

            if (institution != null)
            {
                DepartmentList.DataSource = institution.Departments;
                DepartmentList.DataValueField = "Id";
                DepartmentList.DataTextField = "Name";
                DepartmentList.DataBind();
            }
        }
    }
}