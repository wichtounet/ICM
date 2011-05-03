using System;
using System.Web.UI;
using ICM.Dao;

namespace ICM
{
    public partial class AddInstitution : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var languages = new LanguagesDAO().GetAllLanguages();

            LanguageList.DataSource = languages;
            LanguageList.DataBind();
        }
    }
}