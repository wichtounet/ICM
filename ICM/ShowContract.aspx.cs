using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    public partial class ShowContract : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["contract"] != null)
            {
                var id = Request.QueryString["contract"].ToInt();

                var contract = new ContractsDAO().GetContractById(id);

                IDLabel.Text = contract.Id.ToString();
                TitreLabel.Text = contract.Title;
                dateDebutLabel.Text = contract.Start.ToString("d");
                dateFinLabel.Text = contract.End.ToString("d");
                userLabel.Text = contract.User;
                typeLabel.Text = contract.Type;
                userLabel.Text  = contract.User;
                StateLabel.Text = contract.Archived ? "Oui" : "Non";
                downloadFile.NavigateUrl = "ContractFile.aspx?id=" + contract.fileId.ToString();

                PersonList.DataSource = contract.persons;
                PersonList.DataBind();

                DestinationList.DataSource = contract.departments;
                DestinationList.DataBind();
            }
            else{
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
            } 
        }
        protected void EditContract(object sender, EventArgs e)
        {
            Response.Redirect("AddContract.aspx?contract=" + Request.QueryString["contract"].ToInt()); 
        }

        protected void ArchiveContract(object sender, EventArgs e)
        {
            new ContractsDAO().ArchiveContract(Request.QueryString["contract"].ToInt());
        }

    }
}