using System;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    /// <summary>
    ///  This page enable the users to add or edit a person. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public partial class AddPerson : System.Web.UI.Page
    {
        /// <summary>
        /// Load the informations about the person, load the lists and fill the page. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["person"] != null)
            {
                //In order to not refill the form at postback
                if("-1".Equals(IDLabel.Text) )
                {
                    Extensions.SqlOperation operation = () =>
                    {
                        var id = Request.QueryString["person"].ToInt();

                        var person = new PersonsDAO().GetPersonByID(id);

                        IDLabel.Text = person.Id.ToString();
                        NameTextBox.Text = person.Name;
                        FirstNameTextBox.Text = person.FirstName;
                        MailTextBox.Text = person.Email;
                        PhoneTextBox.Text = person.Phone;

                        SaveButton.Visible = true;

                        var dataSource = new InstitutionsDAO().GetInstitutions();

                        InstitutionList.DataBind(dataSource, "Name", "Id");

                        InstitutionList.SelectedValue = person.Department.InstitutionId.ToString();

                        var institution = new InstitutionsDAO().GetInstitution(person.Department.InstitutionId);

                        DepartmentList.DataBind(institution.Departments, "Name", "Id");
                        
                        DepartmentList.SelectedValue = person.Department.Id.ToString();

                        IDLabel.Text = id.ToString();
                    };

                    this.Verified(operation, ErrorLabel);
                }
            } 
            else
            {
                //In order to not refill the form at postback
                if ("-1".Equals(IDLabel.Text))
                {
                    AddButton.Visible = true;

                    this.Verified(LoadLists, ErrorLabel);

                    IDLabel.Text = "1";
                }
            }
        }

        private void LoadLists()
        {
            var dataSource = new InstitutionsDAO().GetInstitutions();

            InstitutionList.DataBind(dataSource, "Name", "Id");

            if (dataSource.Count > 0)
            {
                DepartmentList.DataBind(dataSource[0].Departments, "Name", "Id");
            }
        }

        /// <summary>
        /// Create a person with the given informations. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void CreatePerson(object sender, EventArgs e)
        {
            if(Page.IsValid)
            {
                Extensions.SqlOperation operation = () =>
                {
                    var departmentId = DepartmentList.SelectedValue.ToInt();

                    var id = new PersonsDAO().CreatePerson(FirstNameTextBox.Text, NameTextBox.Text, PhoneTextBox.Text, MailTextBox.Text, departmentId);

                    Response.Redirect("ShowPerson.aspx?person=" + id);
                };

                this.Verified(operation, ErrorLabel);
            }
        }

        /// <summary>
        /// Save the current person. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void SavePerson(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Extensions.SqlOperation operation = () => 
                {
                    var id = IDLabel.Text.ToInt();

                    var departmentId = DepartmentList.SelectedValue.ToInt();

                    new PersonsDAO().SavePerson(id, FirstNameTextBox.Text, NameTextBox.Text, PhoneTextBox.Text, MailTextBox.Text, departmentId);

                    Response.Redirect("ShowPerson.aspx?person=" + id);
                };

                this.Verified(operation, ErrorLabel);
            }
        }

        /// <summary>
        /// An institution has been selected
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void InstitutionSelected(object sender, EventArgs e)
        {
            Extensions.SqlOperation operation = () =>
            {
                var id = InstitutionList.SelectedValue.ToInt();

                var institution = new InstitutionsDAO().GetInstitution(id);

                if (institution != null)
                {
                    DepartmentList.DataBind(institution.Departments, "Name", "Id");
                }
            };

            this.Verified(operation, ErrorLabel);
        }
    }
}