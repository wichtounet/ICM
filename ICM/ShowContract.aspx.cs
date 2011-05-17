using System;
using System.Web.UI;
using ICM.Dao;
using ICM.Utils;
using System.Xml.Xsl;
using System.Xml;

namespace ICM
{
    /// <summary>
    ///  This page enable the users to view informations about a Contract
    /// </summary>
    /// <remarks>Vincent Ischi</remarks>
    public partial class ShowContract : Page
    {
        /// <summary>
        /// Load the informations about the contract and fill the page. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["contract"] != null)
            {
                var id = Request.QueryString["contract"].ToInt();

                Extensions.SqlOperation operation = () =>
                {
                    var contract = new ContractsDAO().GetContractById(id);

                    IDLabel.Text = contract.Id.ToString();
                    TitreLabel.Text = contract.Title;
                    dateDebutLabel.Text = contract.Start.ToString("yyyy-MM-dd");
                    dateFinLabel.Text = contract.End.ToString("yyyy-MM-dd");//TODO: A CHANGER 
                    userLabel.Text = contract.User;
                    typeLabel.Text = contract.Type;
                    userLabel.Text = contract.User;
                    StateLabel.Text = contract.Archived ? "Oui" : "Non";
                    downloadFile.NavigateUrl = "ContractFile.aspx?id=" + contract.fileId;
                    viewContractXML.NavigateUrl = "ContractOutput.aspx?contract=" + id.ToString();

                    PersonList.DataSource = contract.persons;
                    PersonList.DataBind();

                    DestinationList.DataSource = contract.departments;
                    DestinationList.DataBind();
                };

                this.Verified(operation, ErrorLabel);
            }
            else{
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
            } 
        }


        /// <summary>
        /// Edit the current contract. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void EditContract(object sender, EventArgs e)
        {
            Response.Redirect("AddContract.aspx?contract=" + Request.QueryString["contract"].ToInt()); 
        }

        /// <summary>
        /// Archive the current contract.
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void ArchiveContract(object sender, EventArgs e)
        {
            Extensions.SqlOperation sqlOperation = () =>
            {
                new ContractsDAO().ArchiveContract(Request.QueryString["contract"].ToInt());

                Response.Redirect("ShowContract.aspx?contract=" + Request.QueryString["contract"].ToInt()); 
            };

            this.Verified(sqlOperation, ErrorLabel);
        }
    }
}