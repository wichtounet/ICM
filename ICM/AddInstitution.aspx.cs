﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Dao;
using ICM.Model;

namespace ICM
{
    public partial class AddInstitution : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LanguagesDAO languagesDAO = new LanguagesDAO();

            List<Language> languages = languagesDAO.GetAllLanguages();

            LanguageList.DataSource = languages;
            LanguageList.DataBind();
        }
    }
}