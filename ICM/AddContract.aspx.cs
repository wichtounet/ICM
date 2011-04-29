using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Model;
using ICM.Dao;

namespace ICM
{
    public partial class AddContract : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LanguagesDAO languagesDAO = new LanguagesDAO();

            List<Language> languages = languagesDAO.GetAllLanguages();

            institutionList.DataSource = languages;
            institutionList.DataBind();

            personneList.DataSource = languages;
            personneList.DataBind();
        }
    }
}