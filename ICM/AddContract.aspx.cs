using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using ICM.Model;
using ICM.Dao;
using ICM.Utils;
using System.Collections.Generic;
using System.Collections;
using System.Xml;

namespace ICM
{
    public partial class AddContract : System.Web.UI.Page
    {
        List<Person> personnes;

        protected void Page_Load(object sender, EventArgs e)
        {
            //In order to not refill the form at postback
            if ("-1".Equals(stateForm.Text))
            {
                stateForm.Text = "1";
                
                InstitutionList.DataBindWithEmptyElement(new InstitutionsDAO().GetInstitutions(), "Name", "Id");
                ContractTypeList.DataBindWithEmptyElement(new TypesDAO().GetAllTypes(), "Name", "Name");
                PersonList.DataBindWithEmptyElement(new PersonsDAO().GetAllPersons(), "NameFirstName", "Id");
                RoleList.DataBindWithEmptyElement(new RolesDAO().GetAllRoles(), "Name", "Name");

                if (Request.QueryString["contract"] != null)
                {
                    var id = Request.QueryString["contract"].ToInt();

                    var contract = new ContractsDAO().GetContractById(id);

                    TitleText.Text = contract.Title;
                    StartDate.Text = contract.Start.ToString("d"); // 05/17/2011
                    EndDate.Text = contract.End.ToShortDateString();
                    if (contract.departments.Count != 0)
                    {
                        InstitutionList.SelectedValue = contract.departments[0].InstitutionId.ToString();
                        var institution = new InstitutionsDAO().GetInstitution(contract.departments[0].InstitutionId);
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
                    ContractTypeList.SelectedValue = contract.Type.ToString();

                    FileID.Text = contract.fileId.ToString();
                    downloadFile.NavigateUrl = "ContractFile.aspx?id=" + contract.fileId.ToString();

                    Save.Visible = true;
                }
                else
                {
                    Add.Visible = true;
                    downloadFile.Visible = false;
                }
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

        protected void AddPerson_Click(object sender, EventArgs e)
        {

            if (isValueInList(PersonList.SelectedItem.Value + ";" + RoleList.SelectedItem.Value, PersonSelectedList))
            {
                PersonLabel.Text = "Personne déjà présente dans la liste";
            }
            else
            {
                PersonSelectedList.Items.Add(new ListItem(RoleList.SelectedItem.Text + ": " + PersonList.SelectedItem.Text, PersonList.SelectedItem.Value + ";" + RoleList.SelectedItem.Value));
            }
        }
        protected void DeletePerson_Click(object sender, EventArgs e)
        {
            PersonSelectedList.Items.Remove(PersonSelectedList.SelectedItem);
        }

        protected void AddDepartment_Click(object sender, EventArgs e)
        {
            if(isValueInList(DepartmentList.SelectedItem.Value, DepartmentList))
            {
                DepartmentLabel.Text = "Département déjà présent dans la liste";
            }else
            {
                DepartmentSelectedList.Items.Add(new ListItem(DepartmentList.SelectedItem.Text, DepartmentList.SelectedItem.Value));
            }
            
        }

        private bool isValueInList(string p, DropDownList list)
        {
            for (int i = 0; i < list.Items.Count; i++)
            {
                if (p.Equals(list.Items[i].Value))
                    return true;
            }
            return false;
        }
        protected void DeleteDepartment_Click(object sender, EventArgs e)
        {
            DepartmentSelectedList.Items.Remove(DepartmentSelectedList.SelectedItem);
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Submit();
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Submit();
            }
        }

        private void Submit()
        {
            
            ContractsDAO contractDAO = new ContractsDAO();

            //File
            int fileSize = UploadImageFile.PostedFile.ContentLength;
            string fileMIMEType = UploadImageFile.PostedFile.ContentType;
            BinaryReader fileBinaryReader = new BinaryReader(UploadImageFile.FileContent);
            byte[] fileBinaryBuffer = fileBinaryReader.ReadBytes(fileSize);
            fileBinaryReader.Close();

            //Persons
            SortedList persons = new SortedList();
            for (int i = 0; i < PersonSelectedList.Items.Count; i++)
            {
                string[] w = PersonSelectedList.Items[i].Value.Split(';');
                persons.Add(w[0], w[1]);
            }

            //Destinations
            int[] destination = new int[DepartmentSelectedList.Items.Count];
            for (int i = 0; i < DepartmentSelectedList.Items.Count; i++)
            {
                destination[i] = DepartmentSelectedList.Items[i].Value.ToInt();
            }

            int id;
            XmlDocument xml = createXML();
            if (Request.QueryString["contract"] == null)
            {
                id = contractDAO.AddContract(TitleText.Text, StartDate.Text, EndDate.Text, ContractTypeList.SelectedItem.Value, xml.ToString(), "vincent", persons, destination, fileSize, fileMIMEType, fileBinaryReader, fileBinaryBuffer);
            }
            else
            {
                id = Request.QueryString["contract"].ToInt();
                int contractFileId = -1;
                if (fileSize > 0)
                {
                    contractFileId = contractFileId = FileID.Text.ToInt();

                }
                contractDAO.SaveContract(id, TitleText.Text, StartDate.Text, EndDate.Text, ContractTypeList.SelectedItem.Value, xml.ToString(), "vincent", persons, destination, contractFileId, fileSize, fileMIMEType, fileBinaryReader, fileBinaryBuffer);
            }

            Response.Redirect("showContract.aspx?contract=" + id);
        }

        private XmlDocument createXML()
        {
            int[] departmentTab = new int[PersonSelectedList.Items.Count + DepartmentSelectedList.Items.Count];
            int index = 0;
            
            XmlDocument xmlDoc = new XmlDocument();


            // Write down the XML declaration
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("contract");
            rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootNode.SetAttribute("xsi:noNamespaceSchemaLocation", "contract.xsd");
            rootNode.SetAttribute("title", TitleText.Text);
            rootNode.SetAttribute("startDate", StartDate.Text);
            rootNode.SetAttribute("endDate", EndDate.Text);
            rootNode.SetAttribute("contractType", ContractTypeList.SelectedValue);
            //rootNode.SetAttribute("destinationDepartment", "");
            rootNode.SetAttribute("authorLogin", "");

            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);
            
            

            XmlElement contactsNode = xmlDoc.CreateElement("contacts");
            xmlDoc.DocumentElement.PrependChild(contactsNode);

            PersonsDAO personsDAO = new PersonsDAO();
            for (int i = 0; i < PersonSelectedList.Items.Count; i++)
            {
                string[] w = PersonSelectedList.Items[i].Value.Split(';');
                Person person = personsDAO.GetPersonByID(w[0].ToInt());
                XmlElement personNode = xmlDoc.CreateElement("person");
                personNode.SetAttribute("id", w[0]);
                personNode.SetAttribute("name", person.Name);
                personNode.SetAttribute("firstName", person.FirstName);
                personNode.SetAttribute("phone", person.Phone);
                personNode.SetAttribute("departmentId", person.Department.Id.ToString());
                personNode.SetAttribute("role", w[1]);

                departmentTab[index++] = w[0].ToInt();
                contactsNode.AppendChild(personNode);
            }

            XmlElement destinationsNode = xmlDoc.CreateElement("destinations");
            xmlDoc.DocumentElement.PrependChild(destinationsNode);

            InstitutionsDAO institutionsDAO = new InstitutionsDAO();
            for (int i = 0; i < DepartmentSelectedList.Items.Count; i++)
            {
                int id = DepartmentSelectedList.Items[i].Value.ToInt();
                XmlElement destinationNode = xmlDoc.CreateElement("destination");
                destinationNode.SetAttribute("id", id.ToString());

                departmentTab[index++] = id;
                destinationsNode.AppendChild(destinationNode);
            }

            XmlElement departmentsNode = xmlDoc.CreateElement("departments");
            xmlDoc.DocumentElement.PrependChild(departmentsNode);
            for (int i = 0; i < departmentTab.Length; i++)
            {
                int id = departmentTab[i];
                Department d = institutionsDAO.GetDepartmentById(id);
                XmlElement departmentNode = xmlDoc.CreateElement("department");
                departmentNode.SetAttribute("id", d.Id.ToString());
                departmentNode.SetAttribute("name", d.Name);
                departmentNode.SetAttribute("institutionName", d.InstitutionName);
                departmentNode.SetAttribute("institutionCity", d.InstitutionCity);
                departmentNode.SetAttribute("institutionLanguage", d.InstitutionLanguage);
                departmentNode.SetAttribute("institutionCountry", d.InstitutionCountry);

                departmentsNode.AppendChild(departmentNode);
            }
            
            xmlDoc.Save(Server.MapPath("contract.xml"));

            return xmlDoc;
        }
    }
}