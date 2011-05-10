using System;
using System.Web.UI;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    ///<summary>
    /// This page enable the user to download the file of the contract. 
    ///</summary>
    public partial class ContractFile : Page
    {
        /// <summary>
        /// Download the contract file. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                new ContractsDAO().GetContractFile(Context, Request.QueryString["id"].ToInt());
            }
        }
    }
}