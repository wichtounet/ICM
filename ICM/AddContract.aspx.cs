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

namespace ICM
{
    public partial class AddContract : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PersonsDAO personsDAO = new PersonsDAO();
            TypesDAO typesDAO = new TypesDAO();

            List<Person> personnes = personsDAO.GetAllPersons();
            List<TypeContract> types = typesDAO.GetAllPersons();

            personneList.DataSource = personnes;
            personneList.DataBind();

            institutionList.DataSource = personnes;
            institutionList.DataBind();

            typeContractList.DataSource = types;

            typeContractList.DataBind();
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

            int fileSize = UploadImageFile.PostedFile.ContentLength;
            string fileMIMEType = UploadImageFile.PostedFile.ContentType;
            BinaryReader fileBinaryReader = new BinaryReader(UploadImageFile.FileContent);
            byte[] fileBinaryBuffer = fileBinaryReader.ReadBytes(fileSize);
            fileBinaryReader.Close();

            int id = contractDAO.addContract(titleText.Text, dateDebut.Text, dateFin.Text, "Simple", "vincent", 0, 0, 0, fileSize, fileMIMEType, fileBinaryReader, fileBinaryBuffer);

            Response.Redirect("showContract.aspx?contract="+id);
        }
    }
}