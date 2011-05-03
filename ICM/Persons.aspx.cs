﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    public partial class Persons : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*InstitutionList.DataSource = new InstitutionsDAO().GetInstitutions();
            InstitutionList.DataValueField = "Id";
            InstitutionList.DataTextField = "Name";
            InstitutionList.DataBind();*/
        }

        protected void SearchPerson(object sender, EventArgs e)
        {
            SearchPersons();
        }

        private void SearchPersons()
        {
            var persons = new PersonsDAO().SearchPersons(NameLabel.Text, FirstNameLabel.Text, ArchivedCheckBox.Checked);

            ResultsView.DataSource = persons;
            ResultsView.DataBind();
        }

        protected void PersonDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListViewItem myItem = ResultsView.Items[e.ItemIndex];

            var labelId = myItem.FindControl("LabelID") as Label;

            new PersonsDAO().ArchivePerson(labelId.Text.ToInt());

            SearchPersons();
        }
    }
}