using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using ICM.Dao;
using ICM.Utils;
using NLog;

namespace ICM
{
    /// <summary>
    ///  This page enable the users to add or edit a person. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public partial class AddPerson : System.Web.UI.Page
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static int transactionId;

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

                        var connection = DBManager.GetInstance().GetNewConnection();
                        
                        var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                        
                        new PersonsDAO().LockPerson(id, transaction);

                        var tr = Interlocked.Increment(ref transactionId);

                        Session["connection" + tr] = connection;
                        Session["transaction" + tr] = transaction;

                        ViewState["transaction"] = tr;

                        var person = new PersonsDAO().GetPersonByID(id, transaction);

                        EditAddLabel.Text = "Modification";
                        IDLabel.Text = person.Id.ToString();
                        NameTextBox.Text = person.Name;
                        FirstNameTextBox.Text = person.FirstName;
                        MailTextBox.Text = person.Email;
                        PhoneTextBox.Text = person.Phone;

                        SaveButton.Visible = true;

                        IDLabel.Text = id.ToString();

                        using(var connectionSelect = DBManager.GetInstance().GetNewConnection())
                        {
                            var dataSource = new InstitutionsDAO().GetInstitutions(connectionSelect);

                            InstitutionList.DataBind(dataSource, "Name", "Id");

                            InstitutionList.SelectedValue = person.Department.InstitutionId.ToString();

                            var institution = new InstitutionsDAO().GetInstitution(person.Department.InstitutionId, connectionSelect);

                            DepartmentList.DataBind(institution.Departments, "Name", "Id");

                            DepartmentList.SelectedValue = person.Department.Id.ToString();
                        }
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
                    EditAddLabel.Text = "Nouvelle";

                    this.Verified(LoadLists, ErrorLabel);

                    IDLabel.Text = "1";
                }
            }
        }

        private void LoadLists()
        {
            var dataSource = new InstitutionsDAO().GetInstitutionsClean();

            InstitutionList.DataBind(dataSource, "Name", "Id");

            if (dataSource.Count > 0)
            {
                DepartmentList.DataBind(dataSource[0].Departments, "Name", "Id");
            }
        }

        /// <summary>
        /// Refresh all Lists 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Refresh_Click(object sender, EventArgs e)
        {
            LoadLists();
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

                    var tr = (int) ViewState["transaction"];

                    var transaction = (SqlTransaction) Session["transaction" + tr];
                    var connection = (SqlConnection) Session["connection" + tr];

                    if(transaction == null || connection == null)
                    {
                        Logger.Error("No transaction or connection configured");
                    } 
                    else
                    {
                        new PersonsDAO().SavePerson(id, FirstNameTextBox.Text, NameTextBox.Text, PhoneTextBox.Text, MailTextBox.Text, departmentId, transaction);

                        transaction.Commit();

                        DBManager.GetInstance().CloseConnection(connection);

                        Response.Redirect("ShowPerson.aspx?person=" + id);
                    }
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