﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ICM.Dao;
using ICM.Utils;
using System.Collections;
using System.Xml;
using NLog;

namespace ICM
{
    /// <summary>
    ///  This page enable the users to add or edit a contract. 
    /// </summary>
    /// <remarks>Vincent Ischi</remarks>
    public partial class AddContract : Page
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static int transactionId;

        /// <summary>
        /// Load the contract and all the lists of the page. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //In order to not refill the form at postback
            if ("-1".Equals(stateForm.Text))
            {
                stateForm.Text = "1";

                this.Verified(LoadContract, ErrorLabel);
            }
        }

        /// <summary>
        /// Load the contract and all lists of the page. 
        /// </summary>
        private void LoadContract()
        {
            using (var connectionSelect = DBManager.GetInstance().GetNewConnection())
            {
                InstitutionList.DataBindWithEmptyElement(new InstitutionsDAO().GetInstitutions(connectionSelect), "Name", "Id");
                ContractTypeList.DataBindWithEmptyElement(new TypesDAO().GetAllTypes(connectionSelect), "Name", "Name");
                PersonList.DataBindWithEmptyElement(new PersonsDAO().GetAllPersons(connectionSelect), "NameFirstName", "Id");
                RoleList.DataBindWithEmptyElement(new RolesDAO().GetAllRoles(connectionSelect), "Name", "Name");

                if (Request.QueryString["contract"] != null)
                {
                    var id = Request.QueryString["contract"].ToInt();

                    var connection = DBManager.GetInstance().GetNewConnection();
                    var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                    new ContractsDAO().LockContract(id, transaction);

                    var tr = Interlocked.Increment(ref transactionId);

                    Session["connection" + tr] = connection;
                    Session["transaction" + tr] = transaction;

                    ViewState["transaction"] = tr;

                    var contract = new ContractsDAO().GetContractById(id, transaction);

                    EditAddLabel.Text = "Modification";
                    TitleText.Text = contract.Title;
                    StartValue.Text = contract.Start.ToString("yyyy-MM-dd");
                    EndValue.Text = contract.End.ToString("yyyy-MM-dd");

                    if (contract.departments.Count != 0)
                    {
                        InstitutionList.SelectedValue = contract.departments[0].InstitutionId.ToString();
                        var institution = new InstitutionsDAO().GetInstitution(contract.departments[0].InstitutionId, connectionSelect);

                        if (institution != null)
                        {
                            DepartmentList.DataBindWithEmptyElement(institution.Departments, "Name", "Id");
                        }

                        DepartmentSelectedList.DataSource = contract.departments;
                        DepartmentSelectedList.DataTextField = "Name";
                        DepartmentSelectedList.DataValueField = "Id";
                        DepartmentSelectedList.DataBind();
                    }

                    if (contract.persons.Count != 0)
                    {
                        PersonSelectedList.DataSource = contract.persons;
                        PersonSelectedList.DataTextField = "RoleFirstName";
                        PersonSelectedList.DataValueField = "RoleId";
                        PersonSelectedList.DataBind();
                    }

                    ContractTypeList.SelectedValue = contract.Type;

                    FileID.Text = contract.fileId.ToString();
                    downloadFile.NavigateUrl = "ContractFile.aspx?id=" + contract.fileId;

                    Save.Visible = true;
                }
                else
                {
                    Add.Visible = true;
                    downloadFile.Visible = false;
                    EditAddLabel.Text = "Nouveau";
                }
            }
        }



        /// <summary>
        /// An institution has been selected, loading of departement's list
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void InstitutionSelected(object sender, EventArgs e)
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

        /// <summary>
        /// Add a selected person with his role in the second list
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void AddPerson_Click(object sender, EventArgs e)
        {

            if (isValueInList(PersonList.SelectedItem.Value + ";" + RoleList.SelectedItem.Value, PersonSelectedList))
            {
                PersonLabel.Text = "Personne déjà présente dans la liste";
            }
            else
            {
                if (!"".Equals(RoleList.SelectedItem.Text) && !"".Equals(PersonList.SelectedItem.Text))
                {
                    PersonSelectedList.Items.Add(new ListItem(RoleList.SelectedItem.Text + ": " + PersonList.SelectedItem.Text, PersonList.SelectedItem.Value + ";" + RoleList.SelectedItem.Value));
                }
                else
                {
                    PersonLabel.Text = "Veuillez choisir une personne ET un rôle";
                }
            }
        }

        ///<summary>
        /// Returns the current start date.
        ///</summary>
        ///<returns>The start date on the form of a string. </returns>
        protected string getStartDate()
        {
            return Request.QueryString["contract"] != null ? StartValue.Text : "";
        }

        ///<summary>
        /// Returns the current end date.
        ///</summary>
        ///<returns>The end date on the form of a string. </returns>
        protected string getEndDate()
        {
            return Request.QueryString["contract"] != null ? EndValue.Text : "";
        }

        /// <summary>
        /// Delete a selected person 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void DeletePerson_Click(object sender, EventArgs e)
        {
            PersonSelectedList.Items.Remove(PersonSelectedList.SelectedItem);
        }

        /// <summary>
        /// Add a selected department in the second list
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void AddDepartment_Click(object sender, EventArgs e)
        {
            if(isValueInList(DepartmentList.SelectedItem.Value, DepartmentSelectedList))
            {
                DepartmentLabel.Text = "Département déjà présent dans la liste";
            }
            else
            {
                if (!"".Equals(DepartmentList.SelectedItem.Text))
                {
                    DepartmentSelectedList.Items.Add(new ListItem(DepartmentList.SelectedItem.Text, DepartmentList.SelectedItem.Value));
                }
                
            }
        }

        /// <summary>
        /// Delete a selected person 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void DeleteDepartment_Click(object sender, EventArgs e)
        {
            DepartmentSelectedList.Items.Remove(DepartmentSelectedList.SelectedItem);
        }

        /// <summary>
        /// Check if value is in a ListControl 
        /// </summary>
        /// <param name="p">The value</param>
        /// <param name="list">The ListControl</param>
        private static bool isValueInList(string p, ListControl list)
        {
            for (var i = 0; i < list.Items.Count; i++)
            {
                if (p.Equals(list.Items[i].Value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Submit the form 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Add_Click(object sender, EventArgs e)
        {
            EnableValidator();

            Page.Validate();

            if (Page.IsValid)
            {
                this.Verified(Submit, ErrorLabel);
            }
        }

        /// <summary>
        /// Submit the form 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Save_Click(object sender, EventArgs e)
        {
            EnableValidator();

            Page.Validate();

            if (Page.IsValid)
            {
                this.Verified(Submit, ErrorLabel);
            }
        }

        /// <summary>
        /// Refresh all Lists 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Refresh_Click(object sender, EventArgs e)
        {
            LoadContract();
        }

        /// <summary>
        /// Enable all Validator of form 
        /// </summary>
        private void EnableValidator()
        {
            RequiredTitleValidator.Enabled = true;
            RequiredTypeValidator.Enabled = true;
            CompareDate.Enabled = true;
            RequiredDepartmentValidator.Enabled = true;
            RequiredPersonValidator.Enabled = true;
            CustomValidatorUpload.Enabled = true;
        }

        /// <summary>
        /// Submit the form 
        /// </summary>
        private void Submit()
        {
            //File
            var fileSize = UploadImageFile.PostedFile.ContentLength;
            var fileMIMEType = UploadImageFile.PostedFile.ContentType;
            var fileBinaryReader = new BinaryReader(UploadImageFile.FileContent);
            var fileBinaryBuffer = fileBinaryReader.ReadBytes(fileSize);
            fileBinaryReader.Close();

            //Persons
            var persons = new SortedList();
            for (var i = 0; i < PersonSelectedList.Items.Count; i++)
            {
                var w = PersonSelectedList.Items[i].Value.Split(';');
                persons.Add(w[0], w[1]);
            }

            //Destinations
            var destination = new int[DepartmentSelectedList.Items.Count];
            for (var i = 0; i < DepartmentSelectedList.Items.Count; i++)
            {
                destination[i] = DepartmentSelectedList.Items[i].Value.ToInt();
            }

            var xml = createXML();

            var userName = Session["userLogin"] == null ? "admin" : (string) Session["userLogin"];
            if (Request.QueryString["contract"] == null)
            {
                var id = new ContractsDAO().AddContract(TitleText.Text, StartDate.Text, EndDate.Text, ContractTypeList.SelectedItem.Value, xml.OuterXml, userName, persons, destination, fileSize, fileMIMEType, fileBinaryReader, fileBinaryBuffer);
                
                Response.Redirect("ShowContract.aspx?contract=" + id);
            }
            else
            {
                var id = Request.QueryString["contract"].ToInt();

                var contractFileId = -1;
                if (fileSize > 0)
                {
                    contractFileId = FileID.Text.ToInt();
                }

                var tr = (int) ViewState["transaction"];

                var transaction = (SqlTransaction) Session["transaction" + tr];
                var connection = (SqlConnection) Session["connection" + tr];

                if (transaction == null || connection == null)
                {
                    Logger.Error("No transaction or connection configured");
                }
                else
                {
                    new ContractsDAO().SaveContract(transaction, id, TitleText.Text, StartDate.Text, EndDate.Text, ContractTypeList.SelectedItem.Value, xml.OuterXml, userName, persons, destination, contractFileId, fileSize, fileMIMEType, fileBinaryBuffer);

                    transaction.Commit();

                    DBManager.GetInstance().CloseConnection(connection);

                    Response.Redirect("ShowContract.aspx?contract=" + id);
                }
            }
        }

        /// <summary>
        /// Create a XMLDocument with contract field
        /// </summary>
        /// <returns>A XMLDocument</returns>
        private XmlDocument createXML()
        {
            var departmentTab = new int[PersonSelectedList.Items.Count + DepartmentSelectedList.Items.Count];
            var index = 0;
            
            var xmlDoc = new XmlDocument();

            // Write down the XML declaration
            var xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-16", null);

            // Create the root element
            var rootNode = xmlDoc.CreateElement("contract");
            rootNode.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "contract.xsd"); 

            rootNode.SetAttribute("title", TitleText.Text);
            rootNode.SetAttribute("startDate", StartDate.Text);
            rootNode.SetAttribute("endDate", EndDate.Text);
            rootNode.SetAttribute("contractType", ContractTypeList.SelectedValue);
            rootNode.SetAttribute("authorLogin", "");

            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);
            
            using (var connection = DBManager.GetInstance().GetNewConnection())
            {
                var contactsNode = xmlDoc.CreateElement("contacts");
                xmlDoc.DocumentElement.PrependChild(contactsNode);

                var personsDAO = new PersonsDAO();
                for (var i = 0; i < PersonSelectedList.Items.Count; i++)
                {
                    var w = PersonSelectedList.Items[i].Value.Split(';');
                    var person = personsDAO.GetPersonByID(w[0].ToInt(), connection);
                    var personNode = xmlDoc.CreateElement("person");
                    personNode.SetAttribute("name", person.Name);
                    personNode.SetAttribute("firstName", person.FirstName);
                    personNode.SetAttribute("phone", person.Phone);
                    personNode.SetAttribute("departmentId", person.Department.Id.ToString());
                    personNode.SetAttribute("role", w[1]);

                    departmentTab[index++] = w[0].ToInt();
                    contactsNode.AppendChild(personNode);
                }

                var destinationsNode = xmlDoc.CreateElement("destinations");
                xmlDoc.DocumentElement.PrependChild(destinationsNode);

                for (var i = 0; i < DepartmentSelectedList.Items.Count; i++)
                {
                    var id = DepartmentSelectedList.Items[i].Value.ToInt();

                    var destinationNode = xmlDoc.CreateElement("destination");
                    destinationNode.SetAttribute("id", id.ToString());
                    destinationsNode.AppendChild(destinationNode);

                    departmentTab[index++] = id;
                }

                var institutionsDAO = new InstitutionsDAO();

                var departmentsNode = xmlDoc.CreateElement("departments");
                xmlDoc.DocumentElement.PrependChild(departmentsNode);
                foreach (var id in departmentTab)
                {
                    var d = institutionsDAO.GetDepartmentById(id, connection);
                    var departmentNode = xmlDoc.CreateElement("department");
                    departmentNode.SetAttribute("id", d.Id.ToString());
                    departmentNode.SetAttribute("name", d.Name);
                    departmentNode.SetAttribute("institutionName", d.InstitutionName);
                    departmentNode.SetAttribute("institutionCity", d.InstitutionCity);
                    departmentNode.SetAttribute("institutionLanguage", d.InstitutionLanguage);
                    departmentNode.SetAttribute("institutionCountry", d.InstitutionCountry);

                    departmentsNode.AppendChild(departmentNode);
                }
            }

            return xmlDoc;
        }
    }
}