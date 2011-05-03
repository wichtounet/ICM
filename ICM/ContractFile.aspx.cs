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
    public partial class ContractFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                ContractsDAO contractDAO = new ContractsDAO();
                contractDAO.GetContractFile(Context, Request.QueryString["id"].ToInt());
            }
        }
    }
}