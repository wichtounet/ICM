using System;
using System.Web.UI;

namespace ICM
{
    public partial class Histo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void SearchHisto(object sender, EventArgs e)
        {
            HistoPanel.Visible = true;


        }
    }
}