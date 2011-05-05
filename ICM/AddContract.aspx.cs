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
                PersonList.DataBindWithEmptyElement(new PersonsDAO().GetAllPersons(), "Name", "Id");
                RoleList.DataBindWithEmptyElement(new RolesDAO().GetAllRoles(), "Name", "Name");

                if (Request.QueryString["contract"] != null)
                {
                    var id = Request.QueryString["contract"].ToInt();

                    var contract = new ContractsDAO().GetContractById(id);

                    TitleText.Text = contract.Title;
                    StartDate.Text = contract.Start.ToString("d"); // 05/17/2011
                    EndDate.Text = contract.End.ToShortDateString();
                    //typeContractList
                    //fileID

                    Save.Visible = true;
                }
                else
                {
                    Add.Visible = true;
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
            PersonSelectedList.Items.Add(new ListItem(RoleList.SelectedItem.Text + ": " + PersonList.SelectedItem.Text, PersonList.SelectedItem.Value + ";" + RoleList.SelectedItem.Value));
            //PersonLabel.Text = PersonList.SelectedItem.Text;
        }
        protected void DeletePerson_Click(object sender, EventArgs e)
        {
            PersonSelectedList.Items.Remove(PersonSelectedList.SelectedItem);
        }

        protected void AddDepartment_Click(object sender, EventArgs e)
        {
            DepartmentSelectedList.Items.Add(new ListItem(DepartmentList.SelectedItem.Text, DepartmentList.SelectedItem.Value));
        }
        protected void DeleteDepartment_Click(object sender, EventArgs e)
        {
            DepartmentSelectedList.Items.Remove(DepartmentSelectedList.SelectedItem);
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                submit();
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                submit();
            }
        }

        private void submit()
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
            int[] destination = new int[DepartmentSelectedList.Items.Count];
            for (int i = 0; i < DepartmentSelectedList.Items.Count; i++)
            {
                destination[i] = DepartmentSelectedList.Items[i].Value.ToInt();
            }

            int id = contractDAO.addContract(TitleText.Text, StartDate.Text, EndDate.Text, ContractTypeList.SelectedItem.Value, "vincent", persons, destination, 0, fileSize, fileMIMEType, fileBinaryReader, fileBinaryBuffer);

            Response.Redirect("showContract.aspx?contract="+id);
        }
    }
}