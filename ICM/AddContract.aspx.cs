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

                PersonsDAO personsDAO = new PersonsDAO();
                TypesDAO typesDAO = new TypesDAO();
                RolesDAO roleDAO = new RolesDAO();

                List<TypeContract> types = typesDAO.GetAllPersons();
                List<Role> roles = roleDAO.GetAllRoles();

                personnes = new List<Person>();//personsDAO.GetAllPersons();
                Person p1 = new Person();
                p1.Id = 1;
                p1.Name = "Vincent";
                p1.FirstName = "Ischi";
                personnes.Add(p1);
                Person p2 = new Person();
                p2.Id = 2;
                p2.Name = "Jesus";
                p2.FirstName = "Christ";
                personnes.Add(p2);
                Person p3 = new Person();
                p3.Id = 3;
                p3.Name = "Ta";
                p3.FirstName = "mère";
                personnes.Add(p3);


                personneList.DataSource = personnes;
                personneList.DataValueField = "Id";
                personneList.DataTextField = "Name";
                personneList.DataBind();

                institutionList.DataSource = personnes;
                institutionList.DataBind();

                typeContractList.DataSource = types;
                typeContractList.DataBind();

                roleList.DataSource = roles;
                roleList.DataBind();
            }
            
        }

        protected void addPerson_Click(object sender, EventArgs e)
        {
            choosePersonsList.Items.Add(new ListItem(roleList.SelectedItem.Text + ": " + personneList.SelectedItem.Text, personneList.SelectedItem.Value+";"+roleList.SelectedItem.Value));
            personLabel.Text = personneList.SelectedItem.Text;
        }
        protected void deletePerson_Click(object sender, EventArgs e)
        {
            choosePersonsList.Items.Remove(choosePersonsList.SelectedItem);
        }

        protected void addInstitution_Click(object sender, EventArgs e)
        {
            institutionChooseList.Items.Add(new ListItem(institutionList.SelectedItem.Text, institutionList.SelectedItem.Value));
        }
        protected void deleteInstitution_Click(object sender, EventArgs e)
        {
            institutionChooseList.Items.Remove(institutionChooseList.SelectedItem);
        }
        

        protected void personneList_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selectedItem = personneList.SelectedItem.ToString();

        }


        protected void submitForm_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                submit();
            }
        }

        private void submit()
        {
            
            ContractsDAO contractDAO = new ContractsDAO();

            //contractDAO.addContract(titleText.Text, dateDebut.Text, dateFin.Text, "", "simple", 0, 0, 0, 0);

            //File
            int fileSize = UploadImageFile.PostedFile.ContentLength;
            string fileMIMEType = UploadImageFile.PostedFile.ContentType;
            BinaryReader fileBinaryReader = new BinaryReader(UploadImageFile.FileContent);
            byte[] fileBinaryBuffer = fileBinaryReader.ReadBytes(fileSize);
            fileBinaryReader.Close();

            //Persons
            SortedList persons = new SortedList();
            for (int i = 0; i < choosePersonsList.Items.Count; i++)
            {
                string[] w = choosePersonsList.Items[i].Value.Split(';');
                persons.Add(w[0], w[1]);
            }

            int id = contractDAO.addContract(titleText.Text, dateDebut.Text, dateFin.Text, typeContractList.SelectedItem.Value, "vincent", persons, 0, 0, fileSize, fileMIMEType, fileBinaryReader, fileBinaryBuffer);

            Response.Redirect("showContract.aspx?contract="+id);
        }
    }
}