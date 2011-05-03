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
                dateDebutLabel.Text = contract.Start;
                dateFinLabel.Text = contract.End;
                userLabel.Text = contract.User;
                typeLabel.Text = contract.Type;
                userLabel.Text  = contract.User;
                StateLabel.Text = contract.Archived ? "Oui" : "Non";
                downloadFile.NavigateUrl = "ContractFile.aspx?id=" + contract.fileId.ToString();
            }
            else{
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
            } 
        }
        protected void EditContract(object sender, EventArgs e)
        {
            
        }

        protected void DeleteContract(object sender, EventArgs e)
        {
            
        }

    }
}